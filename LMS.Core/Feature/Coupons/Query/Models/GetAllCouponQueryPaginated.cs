using LMS.Core.Feature.Coupons.Query.Result;
using LMS.Core.Wrappers;
using MediatR;

namespace LMS.Core.Feature.Coupons.Query.Models
{
    public class GetAllCouponQueryPaginated : IRequest<PaginatedResult<GetAllCouponQueryPaginatedResult>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }
}
