using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.Categories.Query.Models;
using LMS.Core.Feature.Categories.Query.Result;
using LMS.Core.Resources;
using LMS.Core.Wrappers;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;
namespace LMS.Core.Feature.Categories.Query.Handler
{
    public class CategoryQueryHandler : ResponseHandler,
        IRequestHandler<GetAllCategoriesQuery, Response<List<GetAllCategoriesQueryResult>>>
        , IRequestHandler<GetCategoryByIdQuery, Response<GetCategoryByIdQueryResult>>
        , IRequestHandler<GetAllCategoriesByFilter, Response<List<AllCategoriesByFilterResponse>>>
        , IRequestHandler<GetAllCategoriesPaginatedQuery, PaginatedResult<GetAllCategoriesPaginatedResult>>

    {
        private readonly IMapper mapper;
        private readonly ICategoriesService categoriesService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public CategoryQueryHandler(IMapper mapper, ICategoriesService categoriesService, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.categoriesService = categoriesService;
            this.stringLocalizer = stringLocalizer;
        }

        public async Task<Response<List<GetAllCategoriesQueryResult>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await categoriesService.GetAllCategories();
            var mappedCategories = mapper.Map<List<GetAllCategoriesQueryResult>>(categories);
            return Success(mappedCategories);
        }

        public async Task<Response<GetCategoryByIdQueryResult>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                return BadRequest<GetCategoryByIdQueryResult>(stringLocalizer[SharedResourcesKeys.InvalidId]);
            }
            var CategoryIsFound = await categoriesService.GetCategoryById(request.Id);
            if (CategoryIsFound == null)
            {
                return NotFound<GetCategoryByIdQueryResult>(stringLocalizer[SharedResourcesKeys.CategoryNotFound]);
            }
            var mappedCategory = mapper.Map<GetCategoryByIdQueryResult>(CategoryIsFound);
            return Success(mappedCategory);
        }

        public async Task<Response<List<AllCategoriesByFilterResponse>>> Handle(GetAllCategoriesByFilter request, CancellationToken cancellationToken)
        {
            var categories = await categoriesService.GetAllCategoriesByFilter(request.Name, request.IsActive);
            var mappedCategories = mapper.Map<List<AllCategoriesByFilterResponse>>(categories);
            return Success(mappedCategories);
        }
        public async Task<PaginatedResult<GetAllCategoriesPaginatedResult>> Handle(GetAllCategoriesPaginatedQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<LMS.Data_.Entities.Categories, GetAllCategoriesPaginatedResult>> expression = e => new GetAllCategoriesPaginatedResult(e.Id, e.Name, e.Description, e.IsActive);
            var quarable = categoriesService.GetAllCategoriesQueryable();
            var PaginatedList = await quarable.Select(expression).ToPaginatedListAsync((int)request.PageNumber, (int)request.PageSize);
            return PaginatedList;

        }
    }
}
