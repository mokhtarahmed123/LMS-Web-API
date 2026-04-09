using FluentValidation;
using LMS.Core.Feature.Lessons.Command.Models;
using LMS.Core.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Lessons.Command.Validators
{
    public class AddLessonValidator : AbstractValidator<AddLessonCommand>
    {
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public AddLessonValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {


            this.stringLocalizer = stringLocalizer;
            ValidateData();
        }

        public void ValidateData()
        {
            RuleFor(x => x.Title)
               .NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
               .MaximumLength(100).WithMessage(stringLocalizer[SharedResourcesKeys.MaximumLengthIs100]);
            RuleFor(x => x.Description)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage(stringLocalizer[SharedResourcesKeys.MaximumLengthIs2000]);

            RuleFor(x => x.Video)
         .NotNull().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
         .Must(BeValidVideo).WithMessage("Invalid video file");


            RuleFor(x => x.DurationMinutes)
        .GreaterThan(0).WithMessage(stringLocalizer[SharedResourcesKeys.GreaterThan0])
        .LessThanOrEqualTo(600).WithMessage(stringLocalizer[SharedResourcesKeys.Durationmustnotexceed10hours]);

            RuleFor(x => x.OrderNumber)
               .GreaterThan(0).WithMessage(stringLocalizer[SharedResourcesKeys.GreaterThan0]);

            RuleFor(x => x.CourseId).NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
               .GreaterThan(0).WithMessage(stringLocalizer[SharedResourcesKeys.GreaterThan0]);
        }



        private bool BeValidVideo(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var allowedExtensions = new[] { ".mp4", ".avi", ".mov", ".mkv" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                return false;

            if (!file.ContentType.StartsWith("video/"))
                return false;

            long maxSize = 400 * 1024 * 1024; // 400MB
            return file.Length <= maxSize;
        }
    }
}
