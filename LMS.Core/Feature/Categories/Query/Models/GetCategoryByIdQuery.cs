using LMS.Core.Feature.Categories.Query.Result;
using MediatR;
namespace LMS.Core.Feature.Categories.Query.Models
{
    public record GetCategoryByIdQuery(int Id) : IRequest<Response<GetCategoryByIdQueryResult>>;
}
