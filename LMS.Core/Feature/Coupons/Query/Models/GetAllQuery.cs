using LMS.Core.Feature.Coupons.Query.Result;
using MediatR;

namespace LMS.Core.Feature.Coupons.Query.Models
{
    public record GetAllQuery(bool IsActive) : IRequest<Response<List<GetAllCouponResult>>>;
}
