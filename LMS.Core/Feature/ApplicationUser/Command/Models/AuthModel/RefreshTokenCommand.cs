using LMS.Data_.Helper;
using MediatR;

namespace LMS.Core.Feature.ApplicationUser.Command.Models.AuthModel
{
    public record RefreshTokenCommand(string RefreshToken, string Token) : IRequest<Response<JWTAuthResponse>>;

}
