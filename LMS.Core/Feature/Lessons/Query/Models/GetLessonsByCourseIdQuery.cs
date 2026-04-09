using LMS.Core.Feature.Lessons.Query.Result;
using MediatR;

namespace LMS.Core.Feature.Lessons.Query.Models
{
    public record GetLessonsByCourseIdQuery(int CourseId) : IRequest<Response<List<GetAllLessonsByCourseIdResult>>>;

}
