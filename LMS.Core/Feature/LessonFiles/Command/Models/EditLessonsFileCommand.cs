using MediatR;
using System.Text.Json.Serialization;

namespace LMS.Core.Feature.LessonFiles.Command.Models
{
    public class EditLessonsFileCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int FileId { get; set; }
        public string FileName { get; set; }
        public int LessonId { get; set; }
    }
}
