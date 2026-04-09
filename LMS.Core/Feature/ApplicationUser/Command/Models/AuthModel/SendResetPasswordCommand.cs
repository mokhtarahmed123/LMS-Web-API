using MediatR;

namespace LMS.Core.Feature.ApplicationUser.Command.Models.AuthModel
{
    public record SendResetPasswordCommand(string Email) : IRequest<Response<string>>;
}
