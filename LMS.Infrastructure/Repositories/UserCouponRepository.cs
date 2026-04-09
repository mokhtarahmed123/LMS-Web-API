using LMS.Data_.Entities;
using LMS.Infrastructure.Abstract;
using LMS.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class UserCouponRepository : GenericRepositoryAsync<UserCoupons>, IUserCouponRepository
    {
        private readonly DbSet<UserCoupons> UserCoupons;
        public UserCouponRepository(AppDbContext dbContext) : base(dbContext)
        {
            UserCoupons = dbContext.Set<UserCoupons>();
        }

        public async Task<bool> IsUsed(string userId, int couponId)
        {
            return await UserCoupons.AnyAsync(uc => uc.UserId == userId && uc.CouponId == couponId);
        }
    }
}
