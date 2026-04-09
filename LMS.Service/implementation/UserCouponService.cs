using LMS.Data_.Entities;
using LMS.Infrastructure.Abstract;
using LMS.Service.Abstract;

namespace LMS.Service.implementation
{
    public class UserCouponService : IUserCouponService
    {
        private readonly IUserCouponRepository userCouponRepository;

        public UserCouponService(IUserCouponRepository userCouponRepository)
        {
            this.userCouponRepository = userCouponRepository;
        }
        public async Task Add(UserCoupons userCoupons)
        {
            await userCouponRepository.AddAsync(userCoupons);
        }

        public Task Delete(UserCoupons userCoupons)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsUsed(string userId, int couponId)
        {
            return await userCouponRepository.IsUsed(userId, couponId);

        }


    }
}
