using FluentValidation;
using LMS.Core.Feature.UserCourses.Command.Models.Student;
using LMS.Core.Resources;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.UserCourses.Command.Validators
{
    public class RateCourseValidator : AbstractValidator<RateUserCourseCommand>
    {
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public RateCourseValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
            ValidateData();
        }
        public void ValidateData()
        {

            RuleFor(x => x.CourseId)
             .NotEmpty()
             .WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
             .GreaterThan(0).WithMessage(stringLocalizer[SharedResourcesKeys.GreaterThan0]);

            RuleFor(x => x.Rating)
                .InclusiveBetween(0, 5)
                .WithMessage(stringLocalizer[SharedResourcesKeys.RateBetween0And5]);



        }
    }
}
