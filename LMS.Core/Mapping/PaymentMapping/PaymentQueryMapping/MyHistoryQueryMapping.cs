using LMS.Core.Payment.Query.Result;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.PaymentMapping
{
    public partial class PaymentProfile
    {
        public void GetMyHistoryQuery()
        {
            CreateMap<Payments, PaymentHistoryItemResult>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.TransactionId))
                .ForMember(dest => dest.SubscriptionId, opt => opt.MapFrom(src => src.SubscriptionId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.user.UserName))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.user.Email))
                .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Subscription.Plan.Name));
        }
    }
}
