using FluentValidation;
using LMS.Core.Feature.InstructorProfiles.Command.Models;
using LMS.Core.Resources;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.InstructorProfiles.Command.Validators
{
    public class AddInstructorProfileCommandValidator : AbstractValidator<AddInstructorProfileCommand>
    {
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public AddInstructorProfileCommandValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
            ValidateData();
        }


        public void ValidateData()
        {
            RuleFor(x => x.Bio)
                .MaximumLength(100)
                .WithMessage(stringLocalizer[SharedResourcesKeys.MaximumLengthIs100]);


            RuleFor(x => x.LinkedInUrl)
                .MaximumLength(2000)
                .WithMessage(stringLocalizer[SharedResourcesKeys.MaximumLengthIs2000])
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage(stringLocalizer[SharedResourcesKeys.LinkedInURL]);

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
                .EmailAddress()
                .WithMessage(stringLocalizer[SharedResourcesKeys.validemailaddress]);
        }
    }
}
