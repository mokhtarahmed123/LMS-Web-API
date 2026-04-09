using MediatR;

namespace LMS.Core.Feature.QuestionOptions.Command.Model
{
    public record DeleteQuestionOptionsCommand(int Id) : IRequest<Response<string>>;

}
