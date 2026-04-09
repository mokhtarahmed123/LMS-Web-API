namespace LMS.Service.BackgroundJobs.Coupon
{

    public interface ICouponJobService
    {
        Task CheckExpiredCoupons();
        Task DeleteExpiredCoupons();
    }

}
