using MediatR;

namespace LMS.Core.Feature.Courses.Command.Models.AdminCommand
{
    public record RejectCourseCommand(int CourseId, string Reasons) : IRequest<Response<string>>;

}
