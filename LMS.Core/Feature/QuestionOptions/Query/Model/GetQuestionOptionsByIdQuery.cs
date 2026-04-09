using LMS.Core.Feature.QuestionOptions.Query.Result;
using MediatR;

namespace LMS.Core.Feature.QuestionOptions.Query.Model
{
    public record GetQuestionOptionsByIdQuery(int Id) : IRequest<Response<GetQuestionOptionsByIdResult>>;


}
