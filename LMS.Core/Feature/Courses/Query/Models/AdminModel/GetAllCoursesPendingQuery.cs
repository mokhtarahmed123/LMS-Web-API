using LMS.Core.Feature.Courses.Query.Result.AdminResultQuery;
using MediatR;

namespace LMS.Core.Feature.Courses.Query.Models.AdminModel
{
    public class GetAllCoursesPendingQuery : IRequest<Response<List<GetAllCoursesPendingResult>>>
    {
    }
}
