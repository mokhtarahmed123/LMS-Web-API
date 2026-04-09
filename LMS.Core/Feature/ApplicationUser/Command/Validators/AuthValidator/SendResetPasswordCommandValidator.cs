using FluentValidation;
using LMS.Core.Feature.ApplicationUser.Command.Models.AuthModel;
using LMS.Core.Resources;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.ApplicationUser.Command.Validators.AuthValidator
{
    public class SendResetPasswordCommandValidator : AbstractValidator<SendResetPasswordCommand>
    {
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public SendResetPasswordCommandValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
            Data();
        }
        public void Data()
        {
            RuleFor(a => a.Email).NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(stringLocalizer[SharedResourcesKeys.Required])
                .EmailAddress().WithMessage(stringLocalizer.GetString(SharedResourcesKeys.validemailaddress));

        }


    }
}
