using LMS.Data_.Entities;

namespace LMS.Service.Abstract
{
    public interface ISubscriptionsService
    {
        public Task<Subscriptions> Add(Subscriptions subscription);
        public Task<bool> Delete(int id);
        public Task<Subscriptions> Get(int id);
        public Task<Subscriptions> Update(Subscriptions subscription);
        public Task<List<Subscriptions>> GetAll();
        Task<bool> IfSubscriptionIsActiveAsync(int SubscriptionId);
        Task<bool> SubscriptionIsAlreadyExitWithUserAndPlan(int PlanId, string UserId);
        Task<List<Subscriptions>> GetAllSubscriptionsAreActivesByUserId(string UserId);
        Task<List<Subscriptions>> GetAllSubscriptionsRequestsByUserId(string UserId);
        Task<int> GetCountOfSubscriptionsByUserId(string UserId);
        Task<decimal> GetAllRevenue();
        Task DeleteAllUserSubscription(string userId);
        Task<Subscriptions> GetSubscriptionWithUser(int SubscriptionId, string UserId);
        Task<List<Subscriptions>> GetMySubscriptions(string UserId);

    }
}
