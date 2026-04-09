using LMS.Core.Feature.Categories.Query.Result;
using MediatR;

namespace LMS.Core.Feature.Categories.Query.Models
{
    public record GetAllCategoriesByFilter : IRequest<Response<List<AllCategoriesByFilterResponse>>>
    {
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
    }
}
