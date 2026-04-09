using LMS.Core.Feature.Subscriptions.Query.Result;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.SubscriptionsMapping
{
    public partial class SubscriptionsProfile
    {
        public void GetMySubscriptions()
        {

            CreateMap<Subscriptions, GetMySubscriptionResult>()
                  .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Id))
                  .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                  .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                  .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                  .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                  .ForMember(dest => dest.SubscriptionStatus, opt => opt.MapFrom(src => src.Status.ToString()))
                  .ForMember(dest => dest.NameOfPlan, opt => opt.MapFrom(src => src.Plan.Name))
                  .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount));



        }
    }
}
