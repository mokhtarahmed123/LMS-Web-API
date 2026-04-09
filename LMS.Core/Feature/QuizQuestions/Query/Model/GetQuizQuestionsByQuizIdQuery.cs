using LMS.Core.Feature.QuizQuestions.Query.Result;
using MediatR;

namespace LMS.Core.Feature.QuizQuestions.Query.Model
{
    public record GetQuizQuestionsByQuizIdQuery(int QuizId) : IRequest<Response<List<GetQuizQuestionsByQuizIdResult>>>;


}
