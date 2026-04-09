namespace LMS.Infrastructure.BackgroundJobs.Subscription
{
    public interface ISubscriptionJobService
    {
        Task CheckExpiredSubscriptions();
        Task NotifyExpiringSubscriptions();
    }
}
