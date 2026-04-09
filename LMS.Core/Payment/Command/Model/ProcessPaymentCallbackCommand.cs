using MediatR;

namespace LMS.Core.Payment.Command.Model
{
    public record ProcessPaymentCallbackCommand(string MerchantOrderId, bool IsSuccess) : IRequest<Response<string>>;
}
