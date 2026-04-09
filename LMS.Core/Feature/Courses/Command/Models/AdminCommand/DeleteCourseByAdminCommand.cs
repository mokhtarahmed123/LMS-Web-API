using MediatR;

namespace LMS.Core.Feature.Courses.Command.Models.AdminCommand
{
    public record DeleteCourseByAdminCommand(int CourseId) : IRequest<Response<string>>;

}
