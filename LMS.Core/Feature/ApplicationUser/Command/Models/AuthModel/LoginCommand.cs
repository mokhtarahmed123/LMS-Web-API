using LMS.Data_.Helper;
using MediatR;

namespace LMS.Core.Feature.ApplicationUser.Command.Models.AuthModel
{
    public record LoginCommand(string Email, string Password) : IRequest<Response<JWTAuthResponse>>;
}
