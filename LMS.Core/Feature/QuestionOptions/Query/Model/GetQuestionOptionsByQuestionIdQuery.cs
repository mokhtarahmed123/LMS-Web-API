using LMS.Core.Feature.QuestionOptions.Query.Result;
using MediatR;

namespace LMS.Core.Feature.QuestionOptions.Query.Model
{
    public record GetQuestionOptionsByQuestionIdQuery(int QuestionId) : IRequest<Response<List<GetQuestionOptionsByQuestionIdResult>>>;

}
