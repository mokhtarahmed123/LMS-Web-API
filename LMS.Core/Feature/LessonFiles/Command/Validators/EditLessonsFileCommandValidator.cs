using FluentValidation;
using LMS.Core.Feature.LessonFiles.Command.Models;
using LMS.Core.Resources;
using LMS.Service.Abstract;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.LessonFiles.Command.Validators
{
    public class EditLessonsFileCommandValidator : AbstractValidator<EditLessonsFileCommand>
    {
        private readonly ILessonsService lessonsService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;


        public EditLessonsFileCommandValidator(ILessonsService lessonsService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.lessonsService = lessonsService;
            this.stringLocalizer = stringLocalizer;
            Validate();
            //ValidateLessonExist();
        }


        public void Validate()
        {

            RuleFor(x => x.FileName)
            .NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
            .MaximumLength(100).WithMessage(stringLocalizer[SharedResourcesKeys.MaximumLengthIs100]);

        }
        //public void ValidateLessonExist()
        //{
        //    RuleFor(x => x.Id).MustAsync(async (id, cancellation) =>
        //    {
        //        var lesson = await lessonsService.GetLessonsById(id);
        //        return lesson != null;
        //    }).WithMessage(stringLocalizer[SharedResourcesKeys.LessonNotFound]);


        //}
    }
}
