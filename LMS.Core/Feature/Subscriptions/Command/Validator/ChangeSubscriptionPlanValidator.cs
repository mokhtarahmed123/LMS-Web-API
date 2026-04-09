using FluentValidation;
using LMS.Core.Feature.Subscriptions.Command.Model;
using LMS.Core.Resources;
using LMS.Service.Abstract;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Subscriptions.Command.Validator
{
    public class ChangeSubscriptionPlanValidator : AbstractValidator<ChangeSubscriptionPlanCommand>
    {
        private readonly IPlanService planService;
        private readonly ISubscriptionsService subscriptionsService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public ChangeSubscriptionPlanValidator(IPlanService planService, ISubscriptionsService subscriptionsService, IStringLocalizer<SharedResources> stringLocalizer)
        {

            this.planService = planService;
            this.subscriptionsService = subscriptionsService;
            this.stringLocalizer = stringLocalizer;
            ValidateData();
        }
        public void ValidateData()
        {
            RuleFor(a => a.PlanId).GreaterThan(0).WithMessage(stringLocalizer[SharedResourcesKeys.GreaterThan0])
                .MustAsync(async (planId, cancellation) =>
                    await planService.PlanIsExistsAsync(planId))
                     .WithMessage(stringLocalizer[SharedResourcesKeys.NotFound])
                ;

            RuleFor(x => x.SubscriptionId)
         .MustAsync(async (subscriptionId, cancellation) =>
             {
                 var subscription = await subscriptionsService.Get(subscriptionId);

                 return subscription != null &&
                        subscription.EndDate > DateOnly.FromDateTime(DateTime.UtcNow);
             })
    .WithMessage(stringLocalizer[SharedResourcesKeys.Cannotchangeplanforanexpiredsubscription]);


        }

    }
}
