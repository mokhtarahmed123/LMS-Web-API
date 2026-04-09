using MediatR;

namespace LMS.Core.Feature.Courses.Command.Models.AdminCommand
{
    public record ApproveCourseCommand(int CourseId) : IRequest<Response<string>>;

}
