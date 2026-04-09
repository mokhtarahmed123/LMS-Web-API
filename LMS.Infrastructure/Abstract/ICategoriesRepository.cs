using LMS.Data_.Entities;

namespace LMS.Infrastructure.Abstract
{
    public interface ICategoriesRepository : IGenericRepositoryAsync<Categories>
    {

        public Task<Categories> GetCategoryByNameAsync(string name);

        public Task<bool> CheckIfThisNameRepeatWithAntherNameWhenidIsdiffrent(string name, int id);

        public Task<List<Categories>> GetAllCategoriesByFilter(string? Name, bool? IsActive);
    }
}
