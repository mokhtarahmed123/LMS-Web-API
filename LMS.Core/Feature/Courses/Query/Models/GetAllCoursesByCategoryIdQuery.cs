using LMS.Core.Feature.Courses.Query.Result;
using MediatR;

namespace LMS.Core.Feature.Courses.Query.Models
{
    public record GetAllCoursesByCategoryIdQuery(int CategoryId) :
        IRequest<Response<List<GetAllCoursesByCategoryIdResult>>>;

}
