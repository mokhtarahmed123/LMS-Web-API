using LMS.Core.Feature.Quizzes.Query.Result;
using MediatR;

namespace LMS.Core.Feature.Quizzes.Query.Model
{
    public record GetQuizByIdQuery(int QuizId, int CourseId) : IRequest<Response<GetQuizByIdResult>>;


}
