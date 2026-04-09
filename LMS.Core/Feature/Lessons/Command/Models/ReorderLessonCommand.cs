using MediatR;

namespace LMS.Core.Feature.Lessons.Command.Models
{
    public record ReorderLessonCommand(int courseId, int orderNumber, int lessonId) : IRequest<Response<string>>
;
}
