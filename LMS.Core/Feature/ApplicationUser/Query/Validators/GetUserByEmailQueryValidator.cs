using FluentValidation;
using LMS.Core.Feature.ApplicationUser.Query.Models.AdminModel;
using LMS.Core.Resources;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.ApplicationUser.Query.Validators
{
    public class GetUserByEmailQueryValidator : AbstractValidator<GetUserByIdQuery>
    {
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public GetUserByEmailQueryValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
            ValidateData();
        }
        public void ValidateData()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty]);

        }

    }
}
