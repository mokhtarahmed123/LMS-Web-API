using MediatR;

namespace LMS.Core.Feature.Authorization.Command.Models
{
    public record DeleteRoleCommand(string Id) : IRequest<Response<string>>;

}
