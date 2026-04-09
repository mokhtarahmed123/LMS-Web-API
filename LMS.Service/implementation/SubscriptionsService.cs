using LMS.Data_.Entities;
using LMS.Infrastructure.Abstract;
using LMS.Service.Abstract;

namespace LMS.Service.implementation
{
    public class SubscriptionsService : ISubscriptionsService
    {
        private readonly ISubscriptionsRepository subscriptionsRepository;

        public SubscriptionsService(ISubscriptionsRepository subscriptionsRepository)
        {
            this.subscriptionsRepository = subscriptionsRepository;
        }
        public async Task<Subscriptions> Add(Subscriptions subscription)
        {
            var Result = await subscriptionsRepository.AddAsync(subscription);
            return Result;

        }

        public async Task<bool> Delete(int id)
        {
            using var transaction = subscriptionsRepository.BeginTransaction();
            try
            {
                var subscription = await subscriptionsRepository.GetByIdAsync(id);

                if (subscription == null)
                    return false;
                await subscriptionsRepository.DeleteAsync(subscription);
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task DeleteAllUserSubscription(string userId)
        {
            using var transaction = subscriptionsRepository.BeginTransaction();
            try
            {
                var subscriptionsActive = await subscriptionsRepository.GetAllSubscriptionsByUser(userId);

                if (subscriptionsActive != null && subscriptionsActive.Any())
                {
                    await subscriptionsRepository.DeleteRangeAsync(subscriptionsActive);
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<Subscriptions> Get(int id)
        {
            return await subscriptionsRepository.GetSubscriptionsByIdAsync(id);
        }
        public async Task<List<Subscriptions>> GetAll()
        {
            return await subscriptionsRepository.GetAllSubscriptionsAsync();
        }

        public Task<decimal> GetAllRevenue()
        {
            return subscriptionsRepository.GetAllRevenue();
        }

        public async Task<List<Subscriptions>> GetAllSubscriptionsAreActivesByUserId(string UserId)
        {
            return await subscriptionsRepository.GetAllSubscriptionsAreActivesByUserId(UserId);
        }

        public async Task<List<Subscriptions>> GetAllSubscriptionsRequestsByUserId(string UserId)
        {
            return await subscriptionsRepository.GetAllSubscriptionsRequestsByUserId(UserId);
        }

        public async Task<int> GetCountOfSubscriptionsByUserId(string UserId)
        {
            return await subscriptionsRepository.GetCountOfSubscriptionsByUserId(UserId);
        }

        public async Task<List<Subscriptions>> GetMySubscriptions(string UserId)
        {
            return await subscriptionsRepository.GetMySubscriptions(UserId);
        }

        public async Task<Subscriptions> GetSubscriptionWithUser(int SubscriptionId, string UserId)
        {
            return await subscriptionsRepository.GetSubscriptionWithUser(SubscriptionId, UserId);
        }

        public async Task<bool> IfSubscriptionIsActiveAsync(int SubscriptionId)
        {
            var Subscription = await subscriptionsRepository.IfSubscriptionIsActiveAsync(SubscriptionId);
            if (Subscription == null) return false;
            return true;
        }
        public async Task<bool> SubscriptionIsAlreadyExitWithUserAndPlan(int PlanId, string UserId)
        {
            var Subscription = await subscriptionsRepository.SubscriptionIsAlreadyExitWithUserAndPlan(PlanId, UserId);
            if (Subscription == null) return false;
            return true;
        }
        public async Task<Subscriptions> Update(Subscriptions subscription)
        {
            await subscriptionsRepository.UpdateAsync(subscription);
            return subscription;
        }


    }
}
