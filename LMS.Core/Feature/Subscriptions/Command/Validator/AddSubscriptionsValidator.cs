using FluentValidation;
using LMS.Core.Feature.Subscriptions.Command.Model;
using LMS.Core.Resources;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Subscriptions.Command.Validator
{
    public class AddSubscriptionsValidator : AbstractValidator<AddSubscriptionsCommand>
    {
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public AddSubscriptionsValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
            ValidateData();
        }

        public void ValidateData()
        {
            RuleFor(a => a.PlanId).NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
                .GreaterThan(0).WithMessage(stringLocalizer[SharedResourcesKeys.GreaterThan0]);
        }
    }
}
