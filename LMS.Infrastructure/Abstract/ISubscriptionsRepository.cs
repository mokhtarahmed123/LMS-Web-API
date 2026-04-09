using LMS.Data_.Entities;

namespace LMS.Infrastructure.Abstract
{
    public interface ISubscriptionsRepository : IGenericRepositoryAsync<Subscriptions>
    {
        public Task<Subscriptions> GetSubscriptionsByIdAsync(int Id);
        public Task<List<Subscriptions>> GetAllSubscriptionsAsync();
        Task<int> GetCountByPlanId(int PlanId);
        public Task<List<Subscriptions>> GetAllByPlanId(int PlanId);
        public Task<Subscriptions> IfSubscriptionIsActiveAsync(int SubscriptionId);

        public Task<Subscriptions> SubscriptionIsAlreadyExitWithUserAndPlan(int PlandId, string UserId);

        public Task<List<Subscriptions>> GetAllSubscriptionsByUser(string UserId);
        public Task<List<Subscriptions>> GetAllSubscriptionsAreActivesByUserId(string UserId);
        public Task<List<Subscriptions>> GetAllSubscriptionsRequestsByUserId(string UserId);
        public Task<int> GetCountOfSubscriptionsByUserId(string UserId);
        public Task<decimal> GetAllRevenue();

        public Task<Subscriptions> GetSubscriptionWithUser(int SubscriptionId, string UserId);

        public Task<List<Subscriptions>> GetMySubscriptions(string UserId);






    }
}
