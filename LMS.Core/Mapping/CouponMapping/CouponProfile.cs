using AutoMapper;

namespace LMS.Core.Mapping.CouponMapping
{
    public partial class CouponProfile : Profile
    {
        public CouponProfile()
        {
            Create();
            Update();
            GetAll();
            GetAllExpired();
            GetByCoupon();
        }
    }
}
