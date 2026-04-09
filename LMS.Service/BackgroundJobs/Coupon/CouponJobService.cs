using LMS.Service.Abstract;
using LMS.Service.BackgroundJobs.Coupon;

public class CouponJobService : ICouponJobService
{
    private readonly ICouponsService _couponService;

    public CouponJobService(ICouponsService couponService)
    {
        _couponService = couponService;
    }

    public async Task CheckExpiredCoupons()
    {
        var now = DateOnly.FromDateTime(DateTime.UtcNow);

        var activeCoupons = await _couponService.GetAllCoupons(IsActive: true);

        var toDeactivate = activeCoupons
            .Where(c => c.EndDate < now ||
                        c.UsedCount >= c.UsageLimit)
            .ToList();

        foreach (var coupon in toDeactivate)
        {
            coupon.IsActive = false;
            await _couponService.Update(coupon);
        }
    }

    public async Task DeleteExpiredCoupons()
    {
        await _couponService.DeleteAllExpiredCoupons();
    }
}