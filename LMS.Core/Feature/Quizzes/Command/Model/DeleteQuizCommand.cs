using MediatR;
namespace LMS.Core.Feature.Quizzes.Command.Model
{
    public record DeleteQuizCommand(int Id) : IRequest<Response<string>>;

}
