using LMS.Core.Feature.ApplicationUser.Query.Result.AdminResult;
using LMS.Core.Wrappers;
using MediatR;

namespace LMS.Core.Feature.ApplicationUser.Query.Models.AdminModel
{
    public record GetAllUserPaginatedQuery(int PageNumber, int PageSize) : IRequest<PaginatedResult<GetAllUserPaginatedResponse>>;

}
