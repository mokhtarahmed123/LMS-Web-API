using MediatR;

namespace LMS.Core.Feature.Lessons.Command.Models
{
    public record DeleteLessonCommand(int LessonId) : IRequest<Response<string>>;

}
