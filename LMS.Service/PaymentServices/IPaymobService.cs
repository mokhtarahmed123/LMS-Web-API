using LMS.Data_.Entities;

namespace LMS.Service
{
    public interface IPaymobService
    {
        public Task<(Subscriptions Subscription, string RedirectUrl)> ProcessPaymentAsync(int SubscriptionId, string paymentMethod);
        public Task<Subscriptions> UpdateSubscriptionsSuccess(string specialReference);
        public Task<Subscriptions> UpdateSubscriptionsFailed(string specialReference);
        public string ComputeHmacSHA512(string data, string secret);
        Task<List<Payments>> MyHistory(string userId);


        Task<List<Payments>> GetAllPaymentCompleted();
        Task<(Subscriptions, string)> ProcessRenewalPaymentAsync(
   int subscribeId, int newPlanId, string paymentMethod);


    }
}
