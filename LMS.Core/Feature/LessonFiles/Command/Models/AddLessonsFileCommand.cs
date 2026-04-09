using MediatR;
using Microsoft.AspNetCore.Http;

namespace LMS.Core.Feature.LessonFiles.Command.Models
{
    public class AddLessonsFileCommand : IRequest<Response<string>>
    {
        public string FileName { get; set; }
        public IFormFile FileUrl { get; set; }
        public int LessonId { get; set; }



    }
}
