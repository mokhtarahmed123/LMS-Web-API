using LMS.Core.Feature.Courses.Query.Result.AdminResultQuery;
using MediatR;

namespace LMS.Core.Feature.Courses.Query.Models.AdminModel
{
    public record GetAllCoursesByInstructorIdQuery(int InstructorId) : IRequest<Response<List<GetAllCoursesByInstructorIdResult>>>

      ;

}
