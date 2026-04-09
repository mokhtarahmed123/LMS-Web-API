using FluentValidation;
using LMS.Core.Feature.Categories.Command.Models;
using LMS.Core.Resources;
using LMS.Service.Abstract;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Categories.Command.Validators
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        private readonly ICategoriesService categoriesService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public UpdateCategoryCommandValidator(ICategoriesService categoriesService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.categoriesService = categoriesService;
            this.stringLocalizer = stringLocalizer;
            updateValidate();
        }

        public void updateValidate()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(100).WithMessage(stringLocalizer[SharedResourcesKeys.MaximumLengthIs100]);


            RuleFor(x => x.Description)
                .MaximumLength(100).WithMessage(stringLocalizer[SharedResourcesKeys.MaximumLengthIs100]);

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(stringLocalizer[SharedResourcesKeys.InvalidId]);

            RuleFor(x => x.Name)
                .MustAsync(async (command, name, cancellation) =>
                {
                    var isDuplicate = await categoriesService.CheckIfThisNameRepeatWithAntherNameWhenidIsdiffrent(name, command.Id);
                    return !isDuplicate;
                }).WithMessage(stringLocalizer[SharedResourcesKeys.CategoryNameMustBeUnique]);
        }
    }
}
