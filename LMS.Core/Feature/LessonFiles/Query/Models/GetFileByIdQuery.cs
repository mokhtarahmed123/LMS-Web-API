using LMS.Core.Feature.LessonFiles.Query.Result;
using MediatR;

namespace LMS.Core.Feature.LessonFiles.Query.Models
{
    public record GetFileByIdQuery(int Id) : IRequest<Response<GetFileByIdResult>>;
}
