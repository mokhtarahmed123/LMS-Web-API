using LMS.Data_.Entities;
using LMS.Infrastructure.Abstract;
using LMS.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class SubscriptionsRepository : GenericRepositoryAsync<Subscriptions>, ISubscriptionsRepository
    {
        private readonly DbSet<Subscriptions> Subscriptions;
        public SubscriptionsRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            Subscriptions = appDbContext.Set<Subscriptions>();
        }
        public async Task<List<Subscriptions>> GetAllByPlanId(int PlanId)
        {
            return await Subscriptions.Where(a => a.PlanId == PlanId).Include(s => s.User).Include(s => s.Plan).OrderByDescending(s => s.StartDate).ToListAsync();
        }

        public async Task<decimal> GetAllRevenue()
        {
            return await Subscriptions
                .Where(s => s.EndDate >= DateOnly.FromDateTime(DateTime.UtcNow) && s.IsActive == true)
                .SumAsync(s => s.Plan.Price);
        }

        public async Task<List<Subscriptions>> GetAllSubscriptionsAreActivesByUserId(string UserId)
        {
            return await Subscriptions.Include(a => a.Plan).Where(a => a.UserId == UserId && a.IsActive == true).ToListAsync();
        }

        public async Task<List<Subscriptions>> GetAllSubscriptionsAsync()
        {
            return await Subscriptions.Include(s => s.User).Include(s => s.Plan).OrderByDescending(s => s.StartDate).ToListAsync();
        }

        public async Task<List<Subscriptions>> GetAllSubscriptionsByUser(string UserId)
        {
            return await Subscriptions.Include(a => a.Plan).Where(a => a.UserId == UserId).ToListAsync();
        }

        public async Task<List<Subscriptions>> GetAllSubscriptionsRequestsByUserId(string UserId)
        {
            return await Subscriptions.Include(a => a.Plan).Where(a => a.UserId == UserId && a.IsActive == false && a.EndDate > DateOnly.FromDateTime(DateTime.UtcNow)).ToListAsync();
        }

        public async Task<int> GetCountByPlanId(int PlanId)
        {
            return await Subscriptions.Where(a => a.PlanId == PlanId).CountAsync();
        }

        public async Task<int> GetCountOfSubscriptionsByUserId(string UserId)
        {
            return await Subscriptions.Where(a => a.UserId == UserId && a.IsActive == true).CountAsync();

        }

        public async Task<List<Subscriptions>> GetMySubscriptions(string UserId)
        {
            return await Subscriptions.Include(a => a.User).Include(a => a.Plan).Where(a => a.UserId == UserId).ToListAsync();

        }

        public async Task<Subscriptions> GetSubscriptionsByIdAsync(int id)
        {
            return await Subscriptions
                .Include(s => s.User)
                .Include(s => s.Plan)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Subscriptions> GetSubscriptionWithUser(int SubscriptionId, string UserId)
        {
            return await Subscriptions.Include(a => a.Plan).Include(s => s.User).FirstOrDefaultAsync(a => a.Id == SubscriptionId && a.UserId == UserId);
        }

        public async Task<Subscriptions> IfSubscriptionIsActiveAsync(int SubscriptionId)
        {
            return await Subscriptions.FirstOrDefaultAsync(a => a.Id == SubscriptionId && a.IsActive == true);
        }



        public async Task<Subscriptions> SubscriptionIsAlreadyExitWithUserAndPlan(int PlandId, string UserId)
        {
            return await Subscriptions.FirstOrDefaultAsync(a => a.PlanId == PlandId && a.UserId == UserId);
        }
    }
}
