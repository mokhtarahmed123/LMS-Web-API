using MediatR;

namespace LMS.Core.Feature.LessonFiles.Command.Models
{
    public record DeleteLessonsFileCommand(int Id) : IRequest<Response<string>>;

}
