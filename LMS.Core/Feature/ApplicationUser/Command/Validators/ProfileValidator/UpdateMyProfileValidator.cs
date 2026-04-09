using FluentValidation;
using LMS.Core.Feature.ApplicationUser.Command.Models.ProfileModel;
using LMS.Core.Resources;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.ApplicationUser.Command.Validators.ProfileValidator
{
    public class UpdateMyProfileValidator : AbstractValidator<UpdateMyProfileCommand>
    {
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        public UpdateMyProfileValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;

            UpdateUserValidatorRule();
        }
        public void UpdateUserValidatorRule()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.Required]);
            RuleFor(x => x.Email).NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.Required]).
                EmailAddress().WithMessage(stringLocalizer[SharedResourcesKeys.validemailaddress]);
        }

    }
}
