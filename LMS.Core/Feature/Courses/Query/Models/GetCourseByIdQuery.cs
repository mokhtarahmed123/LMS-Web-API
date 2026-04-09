using LMS.Core.Feature.Courses.Query.Result;
using MediatR;

namespace LMS.Core.Feature.Courses.Query.Models
{
    public record GetCourseByIdQuery(int CourseId) : IRequest<Response<GetCourseByIdResult>>;

}
