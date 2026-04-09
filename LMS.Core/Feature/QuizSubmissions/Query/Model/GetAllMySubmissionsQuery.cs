using LMS.Core.Feature.QuizSubmissions.Query.Result;
using MediatR;

namespace LMS.Core.Feature.QuizSubmissions.Query.Model
{
    public record GetAllMySubmissionsQuery : IRequest<Response<List<GetAllMySubmissionsResult>>>;

}
