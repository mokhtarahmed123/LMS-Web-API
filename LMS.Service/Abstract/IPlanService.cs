using LMS.Data_.Entities;

namespace LMS.Service.Abstract
{
    public interface IPlanService
    {
        public Task<Plan> Add(Plan plan);
        public Task<bool> Delete(int Id);
        public Task<Plan> Get(int Id);
        public Task<Plan> Update(Plan plan);
        Task<List<Plan>> GetAll();
        Task<bool> PlanIsExistsAsync(int Id);
        Task<Plan> GetPlanById(int Id);


    }
}
