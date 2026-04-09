using MediatR;

namespace LMS.Core.Feature.ApplicationUser.Command.Models.ProfileModel
{
    public record DeleteMyProfileCommand : IRequest<Response<string>>;
}
