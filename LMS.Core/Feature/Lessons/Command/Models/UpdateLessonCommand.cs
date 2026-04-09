using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace LMS.Core.Feature.Lessons.Command.Models
{
    public class UpdateLessonCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public IFormFile Video { get; set; }
        public decimal DurationMinutes { get; set; }
        public int OrderNumber { get; set; }
        public bool IsPreview { get; set; }

        public int CourseId { get; set; }


    }
}
