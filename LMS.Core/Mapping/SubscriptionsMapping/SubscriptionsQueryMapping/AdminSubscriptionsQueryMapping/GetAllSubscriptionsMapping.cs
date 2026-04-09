using LMS.Core.Feature.Subscriptions.Query.Result;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.SubscriptionsMapping
{
    public partial class SubscriptionsProfile
    {
        public void GetAllSubscriptions()
        {

            CreateMap<Subscriptions, GetAllSubscriptionsResult>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.NameofPlan, opt => opt.MapFrom(src => src.Plan.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Plan.Price))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Plan.DurationInMonth));

        }
    }
}
