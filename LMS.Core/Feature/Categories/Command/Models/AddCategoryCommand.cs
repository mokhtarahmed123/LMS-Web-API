using MediatR;
using System.Text.Json.Serialization;

namespace LMS.Core.Feature.Categories.Command.Models
{
    public class AddCategoryCommand : IRequest<Response<string>>
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        [JsonIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
