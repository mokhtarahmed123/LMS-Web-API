using FluentValidation;
using LMS.Core.Feature.ApplicationUser.Command.Models.AuthModel;
using LMS.Core.Resources;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.ApplicationUser.Command.Validators.AuthValidator
{
    public class SignUpUserValidator : AbstractValidator<SignUpUserCommand>
    {
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public SignUpUserValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
            AddUserValidatorRule();

        }


        public void AddUserValidatorRule()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.Required]);
            RuleFor(x => x.Email).NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.Required]).EmailAddress().WithMessage(stringLocalizer[SharedResourcesKeys.validemailaddress]);
            RuleFor(x => x.Password).NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.Required]).MinimumLength(8).WithMessage("Password must be at least 6 characters long.");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.Required]).Equal(x => x.Password).WithMessage(stringLocalizer[SharedResourcesKeys.Confirmpassworddoesnotmatch]);
        }

    }
}
