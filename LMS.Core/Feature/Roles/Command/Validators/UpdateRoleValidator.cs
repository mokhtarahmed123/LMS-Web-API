using FluentValidation;
using LMS.Core.Feature.Authorization.Command.Models;
using LMS.Core.Resources;
using LMS.Service.Abstract;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Authorization.Command.Validators
{
    public class UpdateRoleValidator : AbstractValidator<UpdateRoleCommand>
    {
        private readonly IRoleService authorizationService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public UpdateRoleValidator(IRoleService authorizationService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.authorizationService = authorizationService;
            this.stringLocalizer = stringLocalizer;
            UpdateValidationRules();
        }
        public void UpdateValidationRules()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(100).WithMessage(stringLocalizer[SharedResourcesKeys.NameMustBeBetween3And100]);
        }


        public void NameIsUnique()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MustAsync(async (command, name, cancellationToken) =>
                {
                    var role = await authorizationService.GetRoleByName(name);

                    return role == null || role.Id == command.Id;
                })
                .WithMessage(stringLocalizer[SharedResourcesKeys.RoleNameMustBeUnique]);
        }
    }
}
