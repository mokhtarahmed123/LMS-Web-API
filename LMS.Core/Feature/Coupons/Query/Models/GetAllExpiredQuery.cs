using LMS.Core.Feature.Coupons.Query.Result;
using MediatR;

namespace LMS.Core.Feature.Coupons.Query.Models
{
    public class GetAllExpiredQuery : IRequest<Response<List<GetAllExpiredResult>>>
    {
        //public int Count { get; set; }
    }
}
