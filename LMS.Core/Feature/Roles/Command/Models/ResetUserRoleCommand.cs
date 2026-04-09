using MediatR;

namespace LMS.Core.Feature.Authorization.Command.Models
{
    public record ResetUserRoleCommand(string UserEmail) : IRequest<Response<string>>;

}
