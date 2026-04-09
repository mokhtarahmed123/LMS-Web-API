using LMS.Data_.Entities;

namespace LMS.Infrastructure.Abstract
{
    public interface IUserCouponRepository : IGenericRepositoryAsync<UserCoupons>
    {
        Task<bool> IsUsed(string userId, int couponId);
    }
}
