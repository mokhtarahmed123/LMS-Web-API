using LMS.Core.Feature.Coupons.Query.Result;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.CouponMapping
{
    public partial class CouponProfile
    {
        public void GetAll()
        {
            CreateMap<Coupons, GetAllCouponResult>()
                    .ForMember(a => a.EndDate, opt => opt.MapFrom(a => a.EndDate))
                    .ForMember(a => a.Id, opt => opt.MapFrom(a => a.Id))
                    .ForMember(a => a.Code, opt => opt.MapFrom(a => a.Code))
                      .ForMember(a => a.StartDate, opt => opt.MapFrom(a => a.StartDate))
                     .ForMember(a => a.DiscountValue, opt => opt.MapFrom(a => a.DiscountValue))
                  .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                  .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                  .ForMember(a => a.DiscountType, opt => opt.MapFrom(a => a.DiscountType))
                .ForMember(a => a.IsActive, opt => opt.MapFrom(a => a.IsActive))
                .ForMember(a => a.UsageLimit, opt => opt.MapFrom(a => a.UsageLimit))
                .ForMember(a => a.UsedCount, opt => opt.MapFrom(a => a.UsedCount));


        }
    }
}
