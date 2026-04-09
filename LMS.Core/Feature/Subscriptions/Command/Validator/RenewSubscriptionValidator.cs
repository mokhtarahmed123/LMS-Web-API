using FluentValidation;
using LMS.Core.Feature.Subscriptions.Command.Model;
using LMS.Core.Resources;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Subscriptions.Command.Validator
{
    public class RenewSubscriptionValidator : AbstractValidator<RenewSubscriptionsCommand>
    {
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public RenewSubscriptionValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
            ValidateData();
        }
        public void ValidateData()
        {
            RuleFor(x => x.PlanId)
            .NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
            .GreaterThan(0).WithMessage(stringLocalizer[SharedResourcesKeys.GreaterThan0])
;
            RuleFor(x => x.SubscriptionId)
                .NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
                  .GreaterThan(0).WithMessage(stringLocalizer[SharedResourcesKeys.GreaterThan0]);
        }

    }
}
