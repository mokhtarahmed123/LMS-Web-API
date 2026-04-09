using LMS.Data_.Entities;

namespace LMS.Infrastructure
{
    public interface IPaymobRepository : IGenericRepositoryAsync<Payments>
    {
        public Task<(Subscriptions Subscription, string RedirectUrl)> ProcessPaymentAsync(int enrollmentId, string paymentMethod);
        public Task<Subscriptions> UpdateSubscriptionsSuccess(string specialReference);
        public Task<Subscriptions> UpdateSubscriptionsFailed(string specialReference);


        Task<(Subscriptions, string)> ProcessRenewalPaymentAsync(
           int subscribeId, int newPlanId, string paymentMethod);

        string ComputeHmacSHA512(string data, string secret);


        public Task<List<Payments>> GetAllPaymentCompleted();

        Task<List<Payments>> MyHistory(string userId);


    }
}
