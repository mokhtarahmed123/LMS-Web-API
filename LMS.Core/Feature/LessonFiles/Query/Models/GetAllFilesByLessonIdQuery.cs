using LMS.Core.Feature.LessonFiles.Query.Result;
using MediatR;

namespace LMS.Core.Feature.LessonFiles.Query.Models
{
    public record GetAllFilesByLessonIdQuery(int Id) : IRequest<Response<List<AllLessonFileResult>>>;

}
