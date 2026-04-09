using LMS.Core.Feature.UserCourses.Query.Result;
using MediatR;

namespace LMS.Core.Feature.UserCourses.Query.Models.AdminModel
{
    public record GetAllStudentByCourseIdQuery(int CourseId) : IRequest<Response<List<GetAllStudentByCourseIdResult>>>;

}
