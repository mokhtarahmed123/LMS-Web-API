using LMS.Core.Feature.Quizzes.Query.Result;
using MediatR;

namespace LMS.Core.Feature.Quizzes.Query.Model
{
    public record GetAllQuizzesByCourseIdQuery(int CourseId) : IRequest<Response<List<GetAllQuizzesByCourseIdResult>>>
 ;

}
