using FluentValidation;
using LMS.Core.Feature.UserCourses.Command.Models.Student;
using LMS.Core.Resources;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.UserCourses.Command.Validators
{
    public class EnrollCourseValidator : AbstractValidator<EnrollUserCourseCommand>
    {
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public EnrollCourseValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
            ValidateData();
        }

        public void ValidateData()
        {

            RuleFor(x => x.CourseId)
             .GreaterThan(0)
             .WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty]);

        }

    }
}
