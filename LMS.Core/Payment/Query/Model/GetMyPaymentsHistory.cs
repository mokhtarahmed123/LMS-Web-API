using LMS.Core.Payment.Query.Result;
using MediatR;

namespace LMS.Core.Payment.Query.Model
{
    public record GetMyPaymentsHistory : IRequest<Response<List<PaymentHistoryItemResult>>>;
}
