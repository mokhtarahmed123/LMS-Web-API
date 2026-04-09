using LMS.Data_.Enum;
using MediatR;

namespace LMS.Core.Payment.Command.Model
{
    public class CreatePaymentTokenCommand : IRequest<Response<string>>
    {
        public int SubscribeId { get; set; }
        public PaymentMethodEnum paymentMethod { get; set; }

    }
}
