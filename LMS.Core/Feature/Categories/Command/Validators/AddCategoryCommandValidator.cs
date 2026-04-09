using FluentValidation;
using LMS.Core.Feature.Categories.Command.Models;
using LMS.Core.Resources;
using LMS.Service.Abstract;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Categories.Command.Validators
{
    public class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommand>
    {
        private readonly ICategoriesService categoriesService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public AddCategoryCommandValidator(ICategoriesService categoriesService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.categoriesService = categoriesService;
            this.stringLocalizer = stringLocalizer;
            ValidateAddCategoryCommand();
            UniqueNameValidator();
        }

        public void ValidateAddCategoryCommand()
        {
            RuleFor(x => x.Name)
          .NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
          .MaximumLength(100).WithMessage(stringLocalizer[SharedResourcesKeys.MaximumLengthIs100])
             .Must(name => !string.IsNullOrWhiteSpace(name?.Trim()))
         .WithMessage(stringLocalizer[SharedResourcesKeys.IsNullOrWhiteSpace]);

            RuleFor(x => x.Description)
                .MaximumLength(100).WithMessage(stringLocalizer[SharedResourcesKeys.MaximumLengthIs100]);
        }

        public void UniqueNameValidator()
        {
            RuleFor(x => x.Name)
                .MustAsync(async (name, cancellation) =>
                {
                    var existingCategory = await categoriesService.GetCategoryByName(name);
                    return existingCategory == null;
                })
                .WithMessage(stringLocalizer[SharedResourcesKeys.CategoryNameMustBeUnique]);
        }
    }
}
