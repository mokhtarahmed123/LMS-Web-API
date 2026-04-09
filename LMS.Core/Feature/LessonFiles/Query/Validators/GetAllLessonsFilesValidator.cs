using FluentValidation;
using LMS.Core.Feature.LessonFiles.Query.Models;
using LMS.Core.Resources;
using LMS.Service.Abstract;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.LessonFiles.Query.Validators
{
    public class GetAllLessonsFilesValidator : AbstractValidator<GetAllFilesByLessonIdQuery>
    {
        private readonly ILessonsService lessonsService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public GetAllLessonsFilesValidator(ILessonsService lessonsService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.lessonsService = lessonsService;
            this.stringLocalizer = stringLocalizer;
            ValidateId();
            ValidateLessonExist();
        }
        public void ValidateId()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.Required]).GreaterThan(0).WithMessage(stringLocalizer[SharedResourcesKeys.GreaterThan0]);
        }
        public void ValidateLessonExist()
        {
            RuleFor(x => x.Id).MustAsync(async (id, cancellation) =>
            {
                var lesson = await lessonsService.GetLessonsById(id);
                return lesson != null;
            }).WithMessage(stringLocalizer[SharedResourcesKeys.LessonNotFound]);


        }
    }
}
