using LMS.Data_.Entities;
using LMS.Infrastructure.Abstract;
using LMS.Service.Abstract;
using Microsoft.EntityFrameworkCore;

namespace LMS.Service.implementation
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesRepository categoriesRepository;

        public CategoriesService(ICategoriesRepository categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public async Task<Categories> AddCategory(Categories category)
        {
            return await categoriesRepository.AddAsync(category);
        }

        public Task<bool> CheckIfThisNameRepeatWithAntherNameWhenidIsdiffrent(string name, int id)
        {
            return categoriesRepository.CheckIfThisNameRepeatWithAntherNameWhenidIsdiffrent(name, id);
        }

        public async Task<bool> DeleteCategory(int id)
        {
            using var transaction = categoriesRepository.BeginTransaction();
            try
            {
                var category = await categoriesRepository.GetByIdAsync(id);

                if (category == null)
                    return false;
                await categoriesRepository.DeleteAsync(category);
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }


        public async Task<List<Categories>> GetAllCategories()
        {

            return await categoriesRepository.GetTableNoTracking().ToListAsync();
        }

        public async Task<List<Categories>> GetAllCategoriesByFilter(string? Name, bool? IsActive)
        {
            return await categoriesRepository.GetAllCategoriesByFilter(Name, IsActive);
        }



        public IQueryable<Categories> GetAllCategoriesQueryable()
        {
            return categoriesRepository.GetTableNoTracking().AsQueryable();
        }

        public async Task<Categories> GetCategoryById(int id)
        {
            return await categoriesRepository.GetByIdAsync(id);
        }

        public Task<Categories> GetCategoryByName(string name)
        {
            return categoriesRepository.GetCategoryByNameAsync(name);
        }

        public async Task<string> UpdateCategory(int id, Categories category)
        {
            var existingCategory = GetCategoryById(id);
            if (existingCategory == null)
                return null;
            await categoriesRepository.UpdateAsync(category);
            return "Category updated successfully";

        }
    }
}
