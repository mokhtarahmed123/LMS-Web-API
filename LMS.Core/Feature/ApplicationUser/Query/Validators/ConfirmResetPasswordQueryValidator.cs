using FluentValidation;
using LMS.Core.Feature.ApplicationUser.Query.Models.AuthModel;
using LMS.Core.Resources;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.ApplicationUser.Query.Validators
{
    public class ConfirmResetPasswordQueryValidator : AbstractValidator<ConfirmResetPasswordQuery>
    {
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public ConfirmResetPasswordQueryValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
            ValidateData();
        }
        public void ValidateData()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(stringLocalizer[SharedResourcesKeys.Required])
                ;
            RuleFor(a => a.Email).NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
    .NotNull().WithMessage(stringLocalizer[SharedResourcesKeys.Required])
    .EmailAddress().WithMessage(stringLocalizer.GetString(SharedResourcesKeys.validemailaddress));


        }

    }
}
