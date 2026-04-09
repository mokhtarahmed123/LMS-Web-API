using MediatR;

namespace LMS.Core.Feature.ApplicationUser.Command.Models.AuthModel
{
    public record LogOutCommand : IRequest<Response<string>>;
}
