using MediatR;

namespace LMS.Core.Feature.Plan.Command.Models
{
    public record DeletePlanCommand(int Id) : IRequest<Response<string>>;

}
