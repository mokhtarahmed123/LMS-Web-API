using FluentValidation;
using LMS.Core.Feature.ApplicationUser.Command.Models.ProfileModel;
using LMS.Core.Resources;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.ApplicationUser.Command.Validators.ProfileValidator
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
    {
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public ChangePasswordValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
            ValidateData();
        }
        public void ValidateData()
        {
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.Required]).MinimumLength(8).WithMessage("Password must be at least 6 characters long.");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.Required]).Equal(x => x.NewPassword).WithMessage(stringLocalizer[SharedResourcesKeys.Confirmpassworddoesnotmatch]);

        }
    }
}
