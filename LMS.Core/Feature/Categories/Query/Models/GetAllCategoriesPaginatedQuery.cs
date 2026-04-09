using LMS.Core.Feature.Categories.Query.Result;
using LMS.Core.Wrappers;
using MediatR;

namespace LMS.Core.Feature.Categories.Query.Models
{
    public class GetAllCategoriesPaginatedQuery : IRequest<PaginatedResult<GetAllCategoriesPaginatedResult>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
