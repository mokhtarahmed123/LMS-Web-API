using MediatR;

namespace LMS.Core.Feature.QuizQuestions.Command.Model
{
    public record DeleteQuizQuestionsCommand(int Id) : IRequest<Response<string>>;

}
