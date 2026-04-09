using LMS.Core.Bases;
using LMS.Core.Payment.Command.Model;
using LMS.Core.Resources;
using LMS.Data_;
using LMS.Data_.Enum;
using LMS.Infrastructure.Context;
using LMS.Service;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
namespace LMS.Core.Payment.Command.Handler
{
    public class PaymentCommandHandler : ResponseHandler,
        IRequestHandler<CreatePaymentTokenCommand, Response<string>>,
            IRequestHandler<ProcessPaymentCallbackCommand, Response<string>>


    {
        private readonly IPaymobService paymobService;
        private readonly IConfiguration configuration;
        private readonly ICurrentUserService common;
        private readonly ISubscriptionsService subscriptionsService;

        public PaymentCommandHandler(IPaymobService paymobService, IConfiguration configuration, ICurrentUserService common, ISubscriptionsService subscriptionsService, IStringLocalizer<SharedResources> stringLocalizer, AppDbContext dbContext) : base(stringLocalizer)

        {
            this.paymobService = paymobService;
            this.configuration = configuration;
            this.common = common;
            this.subscriptionsService = subscriptionsService;
            StringLocalizer = stringLocalizer;
            DbContext = dbContext;
        }

        public IStringLocalizer<SharedResources> StringLocalizer { get; }
        public AppDbContext DbContext { get; }

        public async Task<Response<string>> Handle(
     CreatePaymentTokenCommand request, CancellationToken cancellationToken)
        {
            if (request.SubscribeId <= 0)
                return BadRequest<string>("Invalid Subscribe ID.");

            var userId = common.UserIdFromJWT();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized<string>();

            var subscribe = await subscriptionsService
                .GetSubscriptionWithUser(request.SubscribeId, userId);

            if (subscribe == null)
                return NotFound<string>("Subscription not found.");


            if (subscribe.Status != SubscriptionStatusEnum.Pending)
                return BadRequest<string>("Subscription is not in Pending state.");

            try
            {
                if (request.paymentMethod != PaymentMethodEnum.OnlineCard &&
                request.paymentMethod != PaymentMethodEnum.BankTransfer &&
                    request.paymentMethod != PaymentMethodEnum.Wallet &&
                    request.paymentMethod != PaymentMethodEnum.PayPal)
                    return BadRequest<string>("Invalid payment method.");





                var (subscriptionResult, redirectUrl) = await paymobService
                   .ProcessPaymentAsync(request.SubscribeId, request.paymentMethod.ToString());

                return Created<string>(redirectUrl, Meta: new
                {
                    subscriptionId = subscribe.Id,
                    discount = subscribe.Discount,
                    planName = subscribe.Plan.Name,
                    amount = subscribe.Plan.Price * (1 - subscribe.Discount / 100m),
                    PaymentMethod = request.paymentMethod.ToString(),
                    startDate = subscribe.StartDate,
                    endDate = subscribe.EndDate,
                    Email = subscribe.User.Email,
                    IsActive = subscribe.IsActive

                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound<string>(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest<string>(ex.Message);
            }
            catch (Exception ex)
            {
                return ServerError<string>($"Unexpected error: {ex.Message}");
            }
        }

        public async Task<Response<string>> Handle(ProcessPaymentCallbackCommand request, CancellationToken cancellationToken)
        {
            var payment = await DbContext.Payments
            .Include(p => p.Subscription)
            .FirstOrDefaultAsync(p => p.TransactionId == request.MerchantOrderId);
            if (payment == null)
                return NotFound<string>("Payment not found.");
            payment.PaymentStatus = request.IsSuccess
          ? PaymentStatusEnum.Completed
          : PaymentStatusEnum.Failed;

            payment.Subscription.Status = request.IsSuccess
                ? SubscriptionStatusEnum.Success
                : SubscriptionStatusEnum.Failed;

            payment.Subscription.IsActive = request.IsSuccess;

            await DbContext.SaveChangesAsync();

            return Success<string>(request.IsSuccess ? "Payment successful." : "Payment failed.");
        }
    }
}
