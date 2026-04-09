using LMS.Data_.Entities;
using LMS.Infrastructure.Abstract;
using LMS.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class PlanRepository : GenericRepositoryAsync<Plan>, IPlanRepository
    {
        private readonly DbSet<Plan> Plan;
        public PlanRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            Plan = appDbContext.Set<Plan>();

        }

        public async Task<List<Plan>> GetAllPlansAsync()
        {
            return await Plan.ToListAsync();
        }

        public async Task<Plan> GetPlanById(int Id)
        {
            return await Plan.FirstOrDefaultAsync(a => a.Id == Id);
        }
    }
}
