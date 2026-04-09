using MediatR;
using System.Text.Json.Serialization;

namespace LMS.Core.Feature.Categories.Command.Models
{
    public class UpdateCategoryCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }


    }
}
