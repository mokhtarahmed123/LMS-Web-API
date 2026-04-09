using LMS.Core.Feature.Subscriptions.Command.Model;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.SubscriptionsMapping
{
    public partial class SubscriptionsProfile
    {
        public void AddSubscriptions()
        {
            CreateMap<AddSubscriptionsCommand, Subscriptions>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => false))

                .ForMember(dest => dest.PlanId, opt => opt.MapFrom(src => src.PlanId))

                ;
        }
    }
}
