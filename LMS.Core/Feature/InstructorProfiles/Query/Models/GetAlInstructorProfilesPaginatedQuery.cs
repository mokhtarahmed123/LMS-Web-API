using LMS.Core.Feature.InstructorProfiles.Query.Result;
using LMS.Core.Wrappers;
using MediatR;

namespace LMS.Core.Feature.InstructorProfiles.Query.Models
{
    public record GetAlInstructorProfilesPaginatedQuery(int PageNumber, int PageSize) :
        IRequest<PaginatedResult<GetAllInstructorProfilesPaginatedResult>>;

}
