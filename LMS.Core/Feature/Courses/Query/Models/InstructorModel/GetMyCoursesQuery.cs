using LMS.Core.Feature.Courses.Query.Result.InstructorResultQuery;
using MediatR;

namespace LMS.Core.Feature.Courses.Query.Models.InstructorModel
{
    public class GetMyCoursesQuery : IRequest<Response<List<GetMyCoursesResult>>>

    {
    }
}
