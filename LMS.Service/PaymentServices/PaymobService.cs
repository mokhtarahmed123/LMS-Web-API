using LMS.Data_.Entities;
using LMS.Infrastructure;

namespace LMS.Service
{
    public class PaymobService : IPaymobService
    {
        private readonly IPaymobRepository paymobRepository;

        public PaymobService(IPaymobRepository paymobRepository)
        {
            this.paymobRepository = paymobRepository;
        }

        public string ComputeHmacSHA512(string data, string secret)
        {
            return paymobRepository.ComputeHmacSHA512(data, secret);
        }

        public async Task<List<Payments>> GetAllPaymentCompleted()
        {
            return await paymobRepository.GetAllPaymentCompleted();
        }

        public async Task<List<Payments>> MyHistory(string userId)
        {
            return await paymobRepository.MyHistory(userId);

        }

        public async Task<(Subscriptions Subscription, string RedirectUrl)> ProcessPaymentAsync(int SubscribeId, string paymentMethod)
        {
            return await paymobRepository.ProcessPaymentAsync(SubscribeId, paymentMethod);
        }

        public async Task<(Subscriptions, string)> ProcessRenewalPaymentAsync(int subscribeId, int newPlanId, string paymentMethod)
        {
            return await paymobRepository.ProcessRenewalPaymentAsync(subscribeId, newPlanId, paymentMethod);
        }

        public async Task<Subscriptions> UpdateSubscriptionsFailed(string specialReference)
        {
            return await paymobRepository.UpdateSubscriptionsFailed(specialReference);
        }

        public async Task<Subscriptions> UpdateSubscriptionsSuccess(string specialReference)
        {
            return await paymobRepository.UpdateSubscriptionsSuccess(specialReference);
        }
    }
}
