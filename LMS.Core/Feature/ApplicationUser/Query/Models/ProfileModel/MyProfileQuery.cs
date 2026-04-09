using LMS.Core.Feature.ApplicationUser.Query.Result.ProfileResult;
using MediatR;

namespace LMS.Core.Feature.ApplicationUser.Command.Models.ProfileModel
{
    public record MyProfileQuery : IRequest<Response<MyProfileResult>>
  ;
}
