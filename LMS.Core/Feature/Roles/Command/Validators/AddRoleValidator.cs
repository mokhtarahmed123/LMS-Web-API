using FluentValidation;
using LMS.Core.Feature.Authorization.Command.Models;
using LMS.Core.Resources;
using LMS.Data_.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Authorization.Command.Validators
{
    public class AddRoleValidator : AbstractValidator<AddRoleCommand>
    {
        private readonly RoleManager<Role> roleManager;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public AddRoleValidator(RoleManager<Role> roleManager, IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.roleManager = roleManager;
            this.stringLocalizer = stringLocalizer;
            AddValidationRules();
            AddCustomValidation();
        }

        public void AddValidationRules()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(100).WithMessage(stringLocalizer[SharedResourcesKeys.NameMustBeBetween3And100]);
        }

        public void AddCustomValidation()
        {
            RuleFor(x => x.Name)
                .MustAsync(async (name, cancellation) =>
                {
                    var roleExists = await roleManager.RoleExistsAsync(name);
                    return !roleExists;
                })
                .WithMessage(stringLocalizer[SharedResourcesKeys.RoleNameMustBeUnique]);
        }
    }
}
