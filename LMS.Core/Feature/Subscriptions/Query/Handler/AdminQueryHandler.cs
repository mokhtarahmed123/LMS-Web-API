using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.Subscriptions.Query.Model.AdminModel;
using LMS.Core.Feature.Subscriptions.Query.Result;
using LMS.Core.Resources;
using LMS.Data_.Entities;
using LMS.Data_.Enum;
using LMS.Service;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Subscriptions.Query.Handler
{
    public class AdminQueryHandler : ResponseHandler,
        IRequestHandler<GetAllSubscriptionsQuery, Response<GetAllSubscriptionsResponse>>,
        IRequestHandler<GetSubscriptionByUserIdQuery, Response<GetSubscriptionByUserIdResponse>>
        , IRequestHandler<CheckActiveSubscriptionQuery, Response<CheckActiveSubscriptionResult>>
        , IRequestHandler<SummarySubscriptionQuery, Response<SummarySubscriptionResult>>

    {
        private readonly IMapper mapper;
        private readonly IPlanService planService;
        private readonly ISubscriptionsService subscriptionsService;
        private readonly UserManager<Users> userManager;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly IPaymobService paymobService;

        public AdminQueryHandler(IMapper mapper, IPlanService planService, ISubscriptionsService subscriptionsService, UserManager<Users> userManager, IStringLocalizer<SharedResources> stringLocalizer, IPaymobService paymobService) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.planService = planService;
            this.subscriptionsService = subscriptionsService;
            this.userManager = userManager;
            this.stringLocalizer = stringLocalizer;
            this.paymobService = paymobService;
        }


        public async Task<Response<GetAllSubscriptionsResponse>> Handle(GetAllSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            var subscriptions = await subscriptionsService.GetAll();
            var response = new GetAllSubscriptionsResponse
            {
                CountOfActiveSubscriptions = subscriptions.Count(s => s.IsActive),
                CountOfUnActiveSubscriptions = subscriptions.Count(s => !s.IsActive),
                Subscriptions = mapper.Map<List<GetAllSubscriptionsResult>>(subscriptions)
            };
            return Success(response);
        }
        public async Task<Response<GetSubscriptionByUserIdResponse>> Handle(GetSubscriptionByUserIdQuery request, CancellationToken cancellationToken)
        {
            var User = await userManager.FindByIdAsync(request.UserId);
            if (User == null) return NotFound<GetSubscriptionByUserIdResponse>(stringLocalizer[SharedResourcesKeys.UserNotFound]);

            var subscriptions = await subscriptionsService.GetAllSubscriptionsAreActivesByUserId(request.UserId);
            if (!subscriptions.Any()) return NotFound<GetSubscriptionByUserIdResponse>();

            var response = new GetSubscriptionByUserIdResponse
            {
                CountOfSubscriptions = await subscriptionsService.GetCountOfSubscriptionsByUserId(request.UserId),
                GetSubscriptions = mapper.Map<List<GetSubscriptionByUserIdResult>>(subscriptions)
            };
            return Success(response);
        }
        public async Task<Response<CheckActiveSubscriptionResult>> Handle(
            CheckActiveSubscriptionQuery request,
            CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return NotFound<CheckActiveSubscriptionResult>(stringLocalizer[SharedResourcesKeys.UserNotFound]);

            var subscriptions = await subscriptionsService
                .GetAllSubscriptionsAreActivesByUserId(request.UserId);

            if (!subscriptions.Any())
                return NotFound<CheckActiveSubscriptionResult>();

            var subscription = subscriptions.First();

            var response = new CheckActiveSubscriptionResult
            {
                isActive = true,
                endDate = subscription.EndDate
            };

            return Success(response);
        }

        public async Task<Response<SummarySubscriptionResult>> Handle(SummarySubscriptionQuery request, CancellationToken cancellationToken)
        {
            var now = DateOnly.FromDateTime(DateTime.UtcNow);
            var startOfMonth = new DateOnly(now.Year, now.Month, 1);
            var endOfWeek = now.AddDays(7);
            var endOfMonth = new DateOnly(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));


            var subscriptions = await subscriptionsService.GetAll();
            var allPayments = await paymobService.GetAllPaymentCompleted();

            var total = subscriptions.Count;


            var success = subscriptions.Count(s => s.Status == SubscriptionStatusEnum.Success);
            var pending = subscriptions.Count(s => s.Status == SubscriptionStatusEnum.Pending);
            var failed = subscriptions.Count(s => s.Status == SubscriptionStatusEnum.Failed);
            var refunded = subscriptions.Count(s => s.Status == SubscriptionStatusEnum.Refunded);
            var newThisMonth = subscriptions.Count(s => s.StartDate >= startOfMonth && s.StartDate <= now);


            var totalRevenue = allPayments.Sum(p => p.Amount);
            var revenueThisMonth = allPayments
                                    .Where(p => p.PaymentDate >= startOfMonth.ToDateTime(TimeOnly.MinValue))
                                    .Sum(p => p.Amount);
            var avgValue = total > 0 ? totalRevenue / total : 0;
            var totalDiscounts = subscriptions.Sum(s => (decimal)s.Discount);


            var expiringThisWeek = subscriptions.Count(s => s.EndDate >= now && s.EndDate <= endOfWeek);
            var expiringThisMonth = subscriptions.Count(s => s.EndDate >= now && s.EndDate <= endOfMonth);


            var plansSummary = subscriptions
                .GroupBy(s => s.Plan.Name)
                .Select(g => new PlanSummary
                {
                    PlanName = g.Key,
                    Count = g.Count(),
                    Percentage = total > 0 ? Math.Round((double)g.Count() / total * 100, 2) : 0
                })
                .ToList();

            var result = new SummarySubscriptionResult
            {
                TotalSubscriptions = total,
                SuccessSubscriptions = success,
                FailedSubscriptions = failed,
                RefundedSubscriptions = refunded,
                PendingSubscriptions = pending,
                NewSubscriptionsThisMonth = newThisMonth,
                TotalRevenue = totalRevenue,
                RevenueThisMonth = revenueThisMonth,
                AverageSubscriptionValue = avgValue,
                TotalDiscountsGiven = totalDiscounts,
                SubscriptionsExpiringThisWeek = expiringThisWeek,
                SubscriptionsExpiringThisMonth = expiringThisMonth,
                PlansSummary = plansSummary
            };

            return Success(result);
        }
    }
}
