using LMS.Core.Feature.Categories.Query.Result;
using MediatR;

namespace LMS.Core.Feature.Categories.Query
{
    public record GetAllCategoriesQuery : IRequest<Response<List<GetAllCategoriesQueryResult>>>;

}
