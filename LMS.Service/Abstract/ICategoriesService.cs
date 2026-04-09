using LMS.Data_.Entities;

namespace LMS.Service.Abstract
{
    public interface ICategoriesService
    {
        Task<Categories> GetCategoryByName(string name);
        Task<Categories> AddCategory(Categories category);
        Task<List<Categories>> GetAllCategories();
        IQueryable<Categories> GetAllCategoriesQueryable();
        Task<Categories> GetCategoryById(int id);
        Task<string> UpdateCategory(int id, Categories category);
        Task<bool> CheckIfThisNameRepeatWithAntherNameWhenidIsdiffrent(string name, int id);
        Task<bool> DeleteCategory(int id);
        Task<List<Categories>> GetAllCategoriesByFilter(string? Name, bool? IsActive);
    }
}
