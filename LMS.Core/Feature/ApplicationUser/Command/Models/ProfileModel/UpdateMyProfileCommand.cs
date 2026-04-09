using MediatR;

namespace LMS.Core.Feature.ApplicationUser.Command.Models.ProfileModel
{
    public record UpdateMyProfileCommand(string Name, string Email) : IRequest<Response<string>>;

}
