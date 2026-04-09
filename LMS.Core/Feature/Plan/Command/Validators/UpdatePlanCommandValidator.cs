using FluentValidation;
using LMS.Core.Feature.Plan.Command.Models;
using LMS.Core.Resources;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Plan.Command.Validators
{
    public class UpdatePlanCommandValidator : AbstractValidator<UpdatePlanCommand>
    {
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public UpdatePlanCommandValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
            ValidateData();
        }
        public void ValidateData()
        {


            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
                .Length(3, 100).WithMessage(stringLocalizer[SharedResourcesKeys.NameMustBeBetween3And100]);

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage(stringLocalizer[SharedResourcesKeys.GreaterThan0]);

            RuleFor(x => x.DurationInMonths)
                .GreaterThan(0)
                .WithMessage(stringLocalizer[SharedResourcesKeys.GreaterThan0]);
        }
    }
}
