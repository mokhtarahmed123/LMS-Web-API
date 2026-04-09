using FluentValidation;
using LMS.Core.Feature.Coupons.Command.Models;
using LMS.Core.Resources;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Coupons.Command.Validators
{
    public class CreateCouponValidator : AbstractValidator<CreateCouponCommand>
    {
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public CreateCouponValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
            Validate();
        }

        public void Validate()
        {
            RuleFor(a => a.DiscountValue)
               .NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
               .GreaterThan(0).WithMessage(stringLocalizer[SharedResourcesKeys.GreaterThan0]);

            RuleFor(a => a.DiscountType)
                .IsInEnum().WithMessage(stringLocalizer[SharedResourcesKeys.InvalidDiscountType]);

            RuleFor(a => a.StartDate)
                .NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty]);

            RuleFor(a => a.EndDate)
                .NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
                .GreaterThan(a => a.StartDate)
                .WithMessage("End date must be after start date.");

            RuleFor(a => a.UsageLimit)
                .GreaterThan(0).WithMessage(stringLocalizer[SharedResourcesKeys.GreaterThan0]);
            RuleFor(a => a.CountOfCoupons)
                .GreaterThan(0).WithMessage(stringLocalizer[SharedResourcesKeys.GreaterThan0]);





        }
    }
}
