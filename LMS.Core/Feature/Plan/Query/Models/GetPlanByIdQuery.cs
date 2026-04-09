using LMS.Core.Feature.Plan.Query.Result;
using MediatR;

namespace LMS.Core.Feature.Plan.Query.Models
{
    public record GetPlanByIdQuery(int Id) : IRequest<Response<GetPlanByIdResult>>;


}
