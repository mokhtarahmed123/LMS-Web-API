using FluentValidation;
using LMS.Core.Feature.Categories.Command.Models;
using LMS.Core.Resources;
using LMS.Service.Abstract;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Categories.Command.Validators
{
    public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        private readonly ICategoriesService categoriesService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public DeleteCategoryCommandValidator(ICategoriesService categoriesService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.categoriesService = categoriesService;
            this.stringLocalizer = stringLocalizer;
            deleteValidate();
        }

        public void deleteValidate()
        {

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(stringLocalizer[SharedResourcesKeys.InvalidId]);

        }
    }
}
