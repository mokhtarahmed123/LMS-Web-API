using FluentValidation;
using LMS.Core.Feature.Emails.Query.Models;
using LMS.Core.Resources;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Emails.Query.Validators
{
    public class ConfirmEmailQueryValidator : AbstractValidator<ConfirmEmailQuery>
    {
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public ConfirmEmailQueryValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
            Validate();
        }
        public void Validate()
        {
            RuleFor(a => a.UserId).NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(stringLocalizer[SharedResourcesKeys.Required]);
            RuleFor(a => a.Code).NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
                 .NotNull().WithMessage(stringLocalizer[SharedResourcesKeys.Required]);


        }
    }
}
