using MediatR;
using Microsoft.AspNetCore.Http;

namespace LMS.Core.Feature.Lessons.Command.Models
{
    public class AddLessonCommand : IRequest<Response<string>>
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public IFormFile Video { get; set; }
        public decimal DurationMinutes { get; set; }
        public int OrderNumber { get; set; }
        public bool IsPreview { get; set; }

        public int CourseId { get; set; }

    }
}
