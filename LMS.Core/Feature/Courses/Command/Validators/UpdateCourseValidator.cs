using FluentValidation;
using LMS.Core.Feature.Courses.Command.Models.InstructorCommand;
using LMS.Core.Resources;
using LMS.Service.Abstract;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Courses.Command.Validators
{
    public class UpdateCourseValidator : AbstractValidator<UpdateCourseCommand>
    {
        private readonly ICategoriesService categoriesService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public UpdateCourseValidator(ICategoriesService categoriesService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.categoriesService = categoriesService;
            this.stringLocalizer = stringLocalizer;
            ValidateCourse();
            ValidateCategoryId();

        }
        public void ValidateCourse()
        {
            RuleFor(x => x.CourseId)
                 .GreaterThan(0)
                 .WithMessage(stringLocalizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.Title)
         .NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
          .Length(3, 100).WithMessage(stringLocalizer[SharedResourcesKeys.NameMustBeBetween3And100]);
            RuleFor(x => x.Description)
          .MaximumLength(2000).WithMessage(stringLocalizer[SharedResourcesKeys.MaximumLengthIs2000]);

            RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage(stringLocalizer[SharedResourcesKeys.InvalidId]);



            RuleFor(x => x.Level)
                .IsInEnum()
                .WithMessage(stringLocalizer[SharedResourcesKeys.Invalidcourselevel]);

            RuleFor(x => x.CourseLanguage)
                .IsInEnum()
                .WithMessage(stringLocalizer[SharedResourcesKeys.Invalidcourselanguage]);


        }
        public void ValidateCategoryId()
        {
            RuleFor(x => x.CategoryId)
                .MustAsync(async (categoryId, cancellation) =>
                {
                    var category = await categoriesService.GetCategoryById(categoryId);
                    return category != null;
                })
                .WithMessage(stringLocalizer[SharedResourcesKeys.CategoryNotFound]);
        }
    }
}
