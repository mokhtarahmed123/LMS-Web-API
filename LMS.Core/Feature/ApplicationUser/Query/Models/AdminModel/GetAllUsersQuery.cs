using LMS.Core.Feature.ApplicationUser.Query.Result.AdminResult;
using MediatR;

namespace LMS.Core.Feature.ApplicationUser.Query.Models.AdminModel
{
    public record GetAllUsersQuery : IRequest<Response<List<GetUsersQueryResult>>>;
}
