using MediatR;

namespace LMS.Core.Feature.Authorization.Command.Models
{
    public record AddRoleCommand(string Name) : IRequest<Response<string>>;

}
