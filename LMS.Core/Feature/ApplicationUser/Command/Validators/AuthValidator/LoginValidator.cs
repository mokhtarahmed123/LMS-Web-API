using FluentValidation;
using LMS.Core.Feature.ApplicationUser.Command.Models.AuthModel;
using LMS.Core.Resources;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.ApplicationUser.Command.Validators.AuthValidator
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public LoginValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
            Data();
        }
        public void Data()
        {
            RuleFor(a => a.Email).NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(stringLocalizer[SharedResourcesKeys.Required])
                .EmailAddress().WithMessage(stringLocalizer.GetString(SharedResourcesKeys.validemailaddress));
            RuleFor(a => a.Password).NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(stringLocalizer[SharedResourcesKeys.Required])
               ;
        }
    }
}
