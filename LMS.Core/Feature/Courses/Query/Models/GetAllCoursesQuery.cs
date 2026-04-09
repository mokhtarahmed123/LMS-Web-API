using LMS.Core.Feature.Courses.Query.Result;
using MediatR;

namespace LMS.Core.Feature.Courses.Query.Models
{
    public record GetAllCoursesQuery() : IRequest<Response<List<GetAllCoursesResult>>>;

}
