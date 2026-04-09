using AutoMapper;

namespace LMS.Core.Mapping.PaymentMapping
{
    public partial class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            GetMyHistoryQuery();
        }
    }
}
