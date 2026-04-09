using MediatR;

namespace LMS.Core.Feature.Authorization.Command.Models
{
    public record UpdateRoleCommand(string Id, string Name) : IRequest<Response<string>>;

}
