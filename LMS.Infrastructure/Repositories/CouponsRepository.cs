using LMS.Data_.Entities;
using LMS.Infrastructure.Abstract;
using LMS.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class CouponsRepository : GenericRepositoryAsync<Coupons>, ICouponsRepository
    {
        private readonly DbSet<Coupons> Coupons;
        public CouponsRepository(AppDbContext dbContext) : base(dbContext)
        {
            Coupons = dbContext.Set<Coupons>();
        }

        public async Task<int> CountAsync()
        {
            return await Coupons.CountAsync();
        }

        public async Task<List<Coupons>> GetAllAsync(bool IsActive)
        {
            return await Coupons.Where(a => a.IsActive == IsActive).ToListAsync();
        }

        public async Task<List<Coupons>> GetAllExpiredAsync()
        {
            return await Coupons.Where(a => a.EndDate < DateOnly.FromDateTime(DateTime.UtcNow)).ToListAsync();
        }

        public async Task<Coupons> GetByCode(string code)
        {
            return await Coupons.FirstOrDefaultAsync(a => a.Code == code);
        }


    }
}
