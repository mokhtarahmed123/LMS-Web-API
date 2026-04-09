using MediatR;

namespace LMS.Core.Feature.ApplicationUser.Command.Models.AdminModel
{
    public record DeleteUserCommand(string UserId) : IRequest<Response<string>>;
}
