using LMS.Data_.Entities;
using LMS.Service.Abstract;
using MediatR;

namespace LMS.Core.Feature.Courses.Command.Handler
{
    public class ApplyCodeHandler : INotificationHandler<ApplyCodeNotification>
    {
        private readonly ICouponsService couponsService;
        private readonly IUserCouponService userCouponService;

        public ApplyCodeHandler(ICouponsService couponsService, IUserCouponService userCouponService)
        {
            this.couponsService = couponsService;
            this.userCouponService = userCouponService;
        }

        public async Task Handle(ApplyCodeNotification notification, CancellationToken cancellationToken)
        {
            var coupon = await couponsService.GetByCode(notification.Code);
            if (coupon == null) return;



            await userCouponService.Add(new UserCoupons
            {
                UserId = notification.UserId,
                CouponId = coupon.Id,
                UsedAt = DateTime.UtcNow
            });
            coupon.UsedCount += 1;

            if (coupon.UsageLimit > 0)
                coupon.UsageLimit -= 1;

            if (coupon.UsageLimit == 0)
                coupon.IsActive = false;

            await couponsService.Update(coupon);
        }
    }
    public class ApplyCodeNotification : INotification
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string UserId { get; set; }

        public ApplyCodeNotification(string code, int id, string UserId)
        {
            Code = code;
            Id = id;
            this.UserId = UserId;

        }
    }
}
