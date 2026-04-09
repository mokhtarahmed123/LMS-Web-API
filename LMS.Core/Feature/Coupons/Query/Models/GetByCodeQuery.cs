using LMS.Core.Feature.Coupons.Query.Result;
using MediatR;

namespace LMS.Core.Feature.Coupons.Query.Models
{
    public record GetByCodeQuery(string Code) : IRequest<Response<GetByCodeResult>>;
}
