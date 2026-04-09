using MediatR;

namespace LMS.Core.Feature.UserCourses.Command.Models.Student
{
    public record UnenrollCourseCommand(int CourseId) : IRequest<Response<string>>;
}
