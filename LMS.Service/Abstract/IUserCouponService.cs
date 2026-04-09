using LMS.Data_.Entities;

namespace LMS.Service.Abstract
{
    public interface IUserCouponService
    {
        Task Add(UserCoupons userCoupons);

        Task Delete(UserCoupons userCoupons);
        Task<bool> IsUsed(string userId, int couponId);

    }
}
