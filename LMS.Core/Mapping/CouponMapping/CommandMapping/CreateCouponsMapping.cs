using LMS.Core.Feature.Coupons.Command.Models;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.CouponMapping
{
    public partial class CouponProfile
    {
        public void Create()
        {

            CreateMap<CreateCouponCommand, Coupons>()
                .ForMember(a => a.EndDate, opt => opt.MapFrom(a => a.EndDate))
                .ForMember(a => a.StartDate, opt => opt.MapFrom(a => a.StartDate))
                .ForMember(a => a.DiscountValue, opt => opt.MapFrom(a => a.DiscountValue))
                 .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                  .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                  .ForMember(a => a.DiscountType, opt => opt.MapFrom(a => a.DiscountType))
                .ForMember(a => a.IsActive, opt => opt.MapFrom(a => a.IsActive))
                .ForMember(a => a.UsageLimit, opt => opt.MapFrom(a => a.UsageLimit))
                .ForMember(a => a.UsedCount, opt => opt.MapFrom(a => a.UsedCount));



        }
    }
}
