using LMS.Core.Feature.Courses.Query.Result;
using LMS.Core.Wrappers;
using MediatR;

namespace LMS.Core.Feature.Courses.Query.Models
{
    public record GetAllCoursesPaginatedQuery(int PageNumber, int PageSize) : IRequest<PaginatedResult<GetAllCoursesPaginatedResult>>
;
}
