using MediatR;

namespace LMS.Core.Feature.Courses.Command.Models.InstructorCommand
{
    public record DeleteCourseByInstructorCommand(int CourseId) : IRequest<Response<string>>;
}
