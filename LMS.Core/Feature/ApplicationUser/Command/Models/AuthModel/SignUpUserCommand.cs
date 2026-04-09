using MediatR;

namespace LMS.Core.Feature.ApplicationUser.Command.Models.AuthModel
{
    public record SignUpUserCommand(string Name, string Email, string Password, string ConfirmPassword) :
        IRequest<Response<string>>;

}
