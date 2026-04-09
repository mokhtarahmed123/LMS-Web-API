using LMS.Core.Feature.Quizzes.Query.Result;
using MediatR;

namespace LMS.Core.Feature.Quizzes.Query.Model
{
    public record GetAllQuizzesQuery : IRequest<Response<List<GetAllQuizzesResult>>>;


}
