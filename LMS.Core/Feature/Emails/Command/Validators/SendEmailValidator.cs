using FluentValidation;
using LMS.Core.Feature.Emails.Command.Models;
using LMS.Core.Resources;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Emails.Command.Validators
{
    public class SendEmailValidator : AbstractValidator<SendEmailCommand>
    {
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public SendEmailValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
            ValidateData();
        }

        public void ValidateData()
        {
            RuleFor(a => a.Email).NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
            .NotNull().WithMessage(stringLocalizer[SharedResourcesKeys.Required])
           .EmailAddress().WithMessage(stringLocalizer.GetString(SharedResourcesKeys.validemailaddress));

            RuleFor(a => a.Massege).NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
          .NotNull().WithMessage(stringLocalizer[SharedResourcesKeys.Required]);
        }
    }
}
