using LMS.Core.Feature.QuizQuestions.Query.Result;
using MediatR;

namespace LMS.Core.Feature.QuizQuestions.Query.Model
{
    public record GetQuizQuestionByIdQuery(int Id) : IRequest<Response<GetQuizQuestionByIdResult>>;

}
