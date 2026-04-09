using LMS.Data_.Entities;
using LMS.Infrastructure.Abstract;
using LMS.Service.Abstract;

namespace LMS.Service.implementation
{
    public class PlanService : IPlanService
    {
        private readonly IPlanRepository planRepository;

        public PlanService(IPlanRepository planRepository)
        {
            this.planRepository = planRepository;
        }
        public async Task<Plan> Add(Plan plan)
        {
            await planRepository.AddAsync(plan);
            return plan;
        }

        public async Task<bool> Delete(int Id)
        {
            using var transaction = planRepository.BeginTransaction();
            try
            {
                var Plan = await planRepository.GetByIdAsync(Id);
                if (Plan == null)
                    return false;
                await planRepository.DeleteAsync(Plan);
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<Plan> Get(int Id)
        {
            return await planRepository.GetByIdAsync(Id);
        }

        public async Task<List<Plan>> GetAll()
        {
            return await planRepository.GetAllPlansAsync();

        }

        public async Task<Plan> GetPlanById(int Id)
        {
            return await planRepository.GetByIdAsync(Id);

        }

        public async Task<bool> PlanIsExistsAsync(int Id)
        {
            var Plan = await planRepository.GetByIdAsync(Id);
            if (Plan == null) return false;
            return true;

        }

        public async Task<Plan> Update(Plan plan)
        {
            await planRepository.UpdateAsync(plan);
            return plan;
        }
    }
}
