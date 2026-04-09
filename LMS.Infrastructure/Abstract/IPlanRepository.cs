using LMS.Data_.Entities;

namespace LMS.Infrastructure.Abstract
{
    public interface IPlanRepository : IGenericRepositoryAsync<Plan>
    {
        public Task<List<Plan>> GetAllPlansAsync();
        public Task<Plan> GetPlanById(int Id);

    }
}
