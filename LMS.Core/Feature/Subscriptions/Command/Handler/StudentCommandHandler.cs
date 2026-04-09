using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.Courses.Command.Handler;
using LMS.Core.Feature.Subscriptions.Command.Model;
using LMS.Core.Resources;
using LMS.Data_;
using LMS.Data_.Entities;
using LMS.Data_.Enum;
using LMS.Service;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Subscriptions.Command.Handler
{
    public class StudentCommandHandler : ResponseHandler,
        IRequestHandler<AddSubscriptionsCommand, Response<string>>
        , IRequestHandler<ChangeSubscriptionPlanCommand, Response<string>>
        , IRequestHandler<CancelSubscriptionsCommand, Response<string>>
        , IRequestHandler<RenewSubscriptionsCommand, Response<string>>
    {
        private readonly IMapper mapper;
        private readonly ISubscriptionsService subscriptionsService;
        private readonly IPlanService planService;
        private readonly UserManager<Users> userManager;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly ICouponsService couponsService;
        private readonly IMediator mediator;
        private readonly IUserCouponService userCouponService;
        private readonly ICurrentUserService currentUserService;
        private readonly IPaymobService paymobService;

        public StudentCommandHandler(IMapper mapper, ISubscriptionsService subscriptionsService, IPlanService planService, UserManager<Users> userManager, IStringLocalizer<SharedResources> stringLocalizer, ICouponsService couponsService, IMediator mediator, IUserCouponService userCouponService, ICurrentUserService currentUserService, IPaymobService paymobService) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.subscriptionsService = subscriptionsService;
            this.planService = planService;
            this.userManager = userManager;
            this.stringLocalizer = stringLocalizer;
            this.couponsService = couponsService;
            this.mediator = mediator;
            this.userCouponService = userCouponService;
            this.currentUserService = currentUserService;
            this.paymobService = paymobService;
        }
        public async Task<Response<string>> Handle(AddSubscriptionsCommand request, CancellationToken cancellationToken)
        {
            var UserId = currentUserService.UserId;

            var User = await userManager.FindByIdAsync(UserId);
            if (User == null) return NotFound<string>(stringLocalizer[SharedResourcesKeys.UserNotFound]);

            var Plan = await planService.Get(request.PlanId);
            if (Plan == null) return NotFound<string>(stringLocalizer[SharedResourcesKeys.PlanNotFound]);


            var SubscriptionIsAlreadyFound = await subscriptionsService.SubscriptionIsAlreadyExitWithUserAndPlan(request.PlanId, UserId);
            if (SubscriptionIsAlreadyFound) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.SubscriptionIsAlreadyFound]);
            decimal discount = 0;

            var coupon = await couponsService.GetByCode(request.CouponCode);
            if (!string.IsNullOrEmpty(request.CouponCode))
            {
                if (coupon != null && coupon.EndDate > DateOnly.FromDateTime(DateTime.UtcNow) && coupon.IsActive)
                {
                    discount = coupon.DiscountValue;
                }
                else
                {
                    return BadRequest<string>(stringLocalizer[SharedResourcesKeys.InvalidCouponCode]);
                }
            }

            if (coupon != null)
            {
                var alreadyUsed = await userCouponService.IsUsed(UserId, coupon.Id);
                if (alreadyUsed)
                    return BadRequest<string>(stringLocalizer[SharedResourcesKeys.YouHaveAlreadyUsedThisCoupon]);
            }


            var SubscriptionMapped = mapper.Map<LMS.Data_.Entities.Subscriptions>(request);
            var now = DateTime.UtcNow;
            SubscriptionMapped.StartDate = DateOnly.FromDateTime(now);
            SubscriptionMapped.EndDate = DateOnly.FromDateTime(now.AddMonths(Plan.DurationInMonth));
            SubscriptionMapped.IsActive = false;
            SubscriptionMapped.UserId = UserId;
            SubscriptionMapped.Discount = (int)discount;


            var Result = await subscriptionsService.Add(SubscriptionMapped);
            if (Result == null) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedAdded]);

            //  Notify to  Coupon
            if (coupon != null)
            {
                await mediator.Publish(new ApplyCodeNotification(request.CouponCode, coupon.Id, UserId));
            }

            return Created<string>("Subscription created successfully. Please complete your payment to activate your plan.");
        }
        public async Task<Response<string>> Handle(ChangeSubscriptionPlanCommand request, CancellationToken cancellationToken)
        {

            var subscription = await subscriptionsService.Get(request.SubscriptionId);
            if (subscription == null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.SubscriptionNotFound]);

            var plan = await planService.Get(request.PlanId);
            if (plan == null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.PlanNotFound]);

            if (subscription.EndDate <= DateOnly.FromDateTime(DateTime.UtcNow))
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.Subscriptionisexpired]);
            subscription.PlanId = request.PlanId;
            var now = DateTime.UtcNow;
            subscription.StartDate = DateOnly.FromDateTime(now);
            subscription.EndDate = DateOnly.FromDateTime(
                now.AddMonths(plan.DurationInMonth)
            );
            subscription.IsActive = false;
            var result = await subscriptionsService.Update(subscription);
            if (result == null)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedUpdated]);

            return Updated<string>();
        }
        public async Task<Response<string>> Handle(CancelSubscriptionsCommand request, CancellationToken cancellationToken)
        {
            var subscription = await subscriptionsService.Get(request.Id);
            if (subscription == null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.SubscriptionNotFound]);

            if (subscription.IsActive)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.SubscriptionisalreadyActive]);

            subscription.IsActive = false;
            subscription.EndDate = DateOnly.FromDateTime(DateTime.Now);
            await subscriptionsService.Update(subscription);

            return Updated<string>();
        }

        public async Task<Response<string>> Handle(RenewSubscriptionsCommand request, CancellationToken cancellationToken)
        {
            var subscription = await subscriptionsService.Get(request.SubscriptionId);
            if (subscription == null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.SubscriptionNotFound]);

            var plan = await planService.GetPlanById(request.PlanId);
            if (plan == null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.PlanNotFound]);
            var Monthes = plan.DurationInMonth;
            var totalPrice = plan.Price;

            subscription.Status = SubscriptionStatusEnum.Pending;
            await subscriptionsService.Update(subscription);


            var (_, redirectUrl) = await paymobService.ProcessRenewalPaymentAsync(
                subscription.Id,
                request.PlanId,
                request.paymentMethod.ToString()
            );

            return Success(redirectUrl, $"Subscription renewed successfully. Total price: {totalPrice}");
        }
    }

}
