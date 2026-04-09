using AutoMapper;
namespace LMS.Core.Mapping.SubscriptionsMapping
{
    public partial class SubscriptionsProfile : Profile
    {
        public SubscriptionsProfile()
        {
            AddSubscriptions();
            GetMySubscriptions();
            GetAllMyRequestsSubscriptionsQuery();
            GetAllSubscriptions();
            GetSubscriptionByUserId();
        }
    }
}
