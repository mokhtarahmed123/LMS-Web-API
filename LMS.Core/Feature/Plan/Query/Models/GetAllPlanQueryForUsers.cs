using LMS.Core.Feature.Plan.Query.Result;
using MediatR;

namespace LMS.Core.Feature.Plan.Query.Models
{
    public record GetAllPlanQueryForUsers : IRequest<Response<List<GetAllPlanResultForUsers>>>
   ;
}
