using FluentValidation;
using LMS.Core.Feature.LessonFiles.Command.Models;
using LMS.Core.Resources;
using LMS.Service.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.LessonFiles.Command.Validators
{
    public class AddLessonsFileCommandValidator : AbstractValidator<AddLessonsFileCommand>
    {
        private readonly ILessonsService lessonsService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public AddLessonsFileCommandValidator(ILessonsService lessonsService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.lessonsService = lessonsService;
            this.stringLocalizer = stringLocalizer;
            Validate();
        }


        public void Validate()
        {
            RuleFor(x => x.FileName)
            .NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
            .MaximumLength(100).WithMessage(stringLocalizer[SharedResourcesKeys.MaximumLengthIs100]);

            RuleFor(x => x.LessonId).NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])

             .GreaterThan(0).WithMessage(stringLocalizer[SharedResourcesKeys.GreaterThan0])
             .Must(LessonsIsFound).WithMessage(stringLocalizer[SharedResourcesKeys.LessonNotFound]);

            RuleFor(x => x.FileUrl)
              .NotNull().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
            .Must(BeValidfile).WithMessage("Invalid URl file");


        }
        private bool BeValidfile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var allowedExtensions = new[] { ".pdf", ".zip", ".docx", "pptx", ".jpg", ".png", ".xlsx" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                return false;


            long maxSize = 40 * 1024 * 1024; // 40MB
            return file.Length <= maxSize;
        }
        private bool LessonsIsFound(int lessonId)
        {
            return lessonsService.GetLessonsById(lessonId) != null;

        }


    }
}
