using MediatR;

namespace LMS.Core.Feature.ApplicationUser.Command.Models.AuthModel
{
    public record ResetPasswordCommand(string Email, string Password, string ConfirmPassword) : IRequest<Response<string>>;

}
