using MediatR;

namespace LMS.Core.Feature.UserCourses.Command.Models.Student
{
    public record favouriteUserCourseCommand(int CourseId, bool IsFavourite) : IRequest<Response<string>>;


}
