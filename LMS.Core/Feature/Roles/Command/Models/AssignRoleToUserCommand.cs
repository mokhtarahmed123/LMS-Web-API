using MediatR;

namespace LMS.Core.Feature.Authorization.Command.Models
{
    public record AssignRoleToUserCommand(string UserEmail, string RoleName) : IRequest<Response<string>>;

}
