using AutoMapper;
using LMS.Core;
using LMS.Core.Bases;
using LMS.Core.Feature.Categories.Command.Models;
using LMS.Core.Resources;
using LMS.Data_.Entities;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;
namespace LMS
{
    public class CategoryCommandHandler : ResponseHandler,
        IRequestHandler<AddCategoryCommand, Response<string>>,
        IRequestHandler<DeleteCategoryCommand, Response<string>>
        , IRequestHandler<UpdateCategoryCommand, Response<string>>
    {
        private readonly ICategoriesService categoriesService;
        private readonly IMapper mapper;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public CategoryCommandHandler(ICategoriesService categoriesService, IMapper mapper, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            this.categoriesService = categoriesService;
            this.mapper = mapper;
            this.stringLocalizer = stringLocalizer;
        }
        public async Task<Response<string>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var mappedCategory = mapper.Map<Categories>(request);
            var response = await categoriesService.AddCategory(mappedCategory);
            if (response == null) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedAdded]);
            return Created<string>(stringLocalizer[SharedResourcesKeys.CategoryAdded]);
        }

        public async Task<Response<string>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await categoriesService.GetCategoryById(request.Id);
            if (category == null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.CategoryNotFound]);
            var response = await categoriesService.DeleteCategory(request.Id);
            if (!response) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedDeleted]);
            return Deleted<string>(stringLocalizer[SharedResourcesKeys.CategoryDeleted]);
        }

        public async Task<Response<string>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var Category = await categoriesService.GetCategoryById(request.Id);
            if (Category == null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.CategoryNotFound]);
            var mappedCategory = mapper.Map(request, Category);
            var response = await categoriesService.UpdateCategory(request.Id, mappedCategory);
            if (response == null) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedUpdated]);
            return Updated<string>(stringLocalizer[SharedResourcesKeys.CategoryUpdated]);

        }
    }
}
