using LMS.Data_.Entities;
using LMS.Infrastructure.Abstract;
using LMS.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class CategoriesRepository : GenericRepositoryAsync<Categories>, ICategoriesRepository
    {

        private readonly DbSet<Categories> CategoryRepo;
        public CategoriesRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            CategoryRepo = appDbContext.Set<Categories>();

        }

        public Task<bool> CheckIfThisNameRepeatWithAntherNameWhenidIsdiffrent(string name, int id)
        {
            return CategoryRepo
                   .AsTracking()
                .AnyAsync(c => c.Name == name && c.Id != id);
        }

        public async Task<List<Categories>> GetAllCategoriesByFilter(string? Name, bool? IsActive)
        {
            return await CategoryRepo
                .Where(a =>
                    (Name == null || a.Name.StartsWith(Name)) &&
                    (IsActive == null || a.IsActive == IsActive)
                )
                .ToListAsync();
            //|| a.Name == Name 

        }

        public async Task<Categories> GetCategoryByNameAsync(string name)
        {
            return await CategoryRepo
                   .AsTracking()
                .FirstOrDefaultAsync(c => c.Name == name);
        }

    }
}
