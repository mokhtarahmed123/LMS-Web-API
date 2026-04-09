
using LMS.Data_.Enum;
using LMS.Service.Abstract;
using LMS.Service.EmailServices;

namespace LMS.Infrastructure.BackgroundJobs.Subscription
{
    public class SubscriptionJobService : ISubscriptionJobService
    {
        private readonly ISubscriptionsService _subscriptionsService;
        private readonly IEmailService emailService;

        public SubscriptionJobService(ISubscriptionsService subscriptionsService, IEmailService emailService)
        {
            _subscriptionsService = subscriptionsService;
            this.emailService = emailService;
        }
        public async Task CheckExpiredSubscriptions()
        {
            var now = DateOnly.FromDateTime(DateTime.UtcNow);

            var subscriptions = await _subscriptionsService.GetAll();

            var expired = subscriptions
                .Where(s => s.EndDate < now && s.Status != SubscriptionStatusEnum.Failed)
                .ToList();

            foreach (var sub in expired)
            {
                sub.Status = SubscriptionStatusEnum.Failed;
                sub.IsActive = false;
                await _subscriptionsService.Update(sub);
            }
        }

        public async Task NotifyExpiringSubscriptions()
        {
            var now = DateOnly.FromDateTime(DateTime.UtcNow);
            var sevenDaysLater = now.AddDays(7);

            var subscriptions = await _subscriptionsService.GetAll();

            var expiringSoon = subscriptions
                .Where(s => s.Status == SubscriptionStatusEnum.Success &&
                            s.EndDate >= now &&
                            s.EndDate <= sevenDaysLater)
                .ToList();

            foreach (var sub in expiringSoon)
            {
                var daysLeft = sub.EndDate.DayNumber - now.DayNumber;

                await emailService.SendEmailAsync(
                    sub.User.Email,
                    $"اشتراكك في خطة {sub.Plan.Name} سينتهي خلال {daysLeft} أيام. قم بالتجديد الآن للاستمرار في الاستفادة من الخدمة.",
                    "تنبيه انتهاء الاشتراك"
                );
            }
        }
    }
}
