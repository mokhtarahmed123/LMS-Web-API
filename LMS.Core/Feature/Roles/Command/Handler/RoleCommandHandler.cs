using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.Authorization.Command.Models;
using LMS.Core.Resources;
using LMS.Data_.Entities;
using LMS.Data_.Enum;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System.Data;
namespace LMS.Core.Feature.Authorization.Command.Handler
{
    public record InstructorRoleApprovedEvent(Guid UserId) : INotification;
    public record InstructorRoleRejectedEvent(Guid UserId, string? Reason) : INotification;
    public class RoleCommandHandler : ResponseHandler,
        IRequestHandler<AddRoleCommand, Response<string>>
        , IRequestHandler<UpdateRoleCommand, Response<string>>
    , IRequestHandler<DeleteRoleCommand, Response<string>>,
        IRequestHandler<AssignRoleToUserCommand, Response<string>>,
        IRequestHandler<ResetUserRoleCommand, Response<string>>,
        IRequestHandler<AcceptInstructorRoleRequestCommand, Response<string>>,
        IRequestHandler<RejectInstructorRoleRequestCommand, Response<string>>
    {
        private readonly IMapper mapper;
        private readonly RoleManager<Role> roleManager;
        private readonly IRoleService authorizationService;
        private readonly UserManager<Users> userManger;
        private readonly IInstructorProfilesService instructorProfilesService;
        private readonly IMediator mediator;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public RoleCommandHandler(IMapper mapper,
            RoleManager<Role> roleManager,
            IRoleService authorizationService,
            UserManager<Users> userManger, IInstructorProfilesService instructorProfilesService, IMediator mediator, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.roleManager = roleManager;
            this.authorizationService = authorizationService;
            this.userManger = userManger;
            this.instructorProfilesService = instructorProfilesService;
            this.mediator = mediator;
            this.stringLocalizer = stringLocalizer;
        }
        public async Task<Response<string>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            var IdentityRole = new Role();
            IdentityRole.Name = request.Name;
            var role = await roleManager.CreateAsync(IdentityRole);
            if (role.Succeeded)
            {
                return Created<string>(request.Name);
            }
            else
            {
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedAdded]);
            }
        }

        public async Task<Response<string>> Handle(
            UpdateRoleCommand request,
            CancellationToken cancellationToken)
        {
            var role = await authorizationService.GetRoleById(request.Id);
            if (role == null)
                return NotFound<string>();
            var roleWithSameName =
                await authorizationService.GetRoleByName(request.Name);

            if (roleWithSameName != null && roleWithSameName.Id != role.Id)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.RoleNameMustBeUnique]);

            var IfRoleIsAssignedToAnyUser = await userManger.GetUsersInRoleAsync(role.Name);
            if (IfRoleIsAssignedToAnyUser.Count > 0)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.Cannotupdaterolename]);

            role.Name = request.Name;
            role.NormalizedName = request.Name.ToUpperInvariant();

            var result = await roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ",
                    result.Errors.Select(e => e.Description));

                return BadRequest<string>(errors);
            }

            return Updated<string>();
        }

        public async Task<Response<string>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var RoleIsFound = await authorizationService.GetRoleById(request.Id);
            if (RoleIsFound == null)
                return NotFound<string>();


            var IfRoleIsAssignedToAnyUser = await userManger.GetUsersInRoleAsync(RoleIsFound.Name);
            if (IfRoleIsAssignedToAnyUser.Count > 0)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.CannotDeleterolename]);


            var result = await roleManager.DeleteAsync(RoleIsFound);
            if (result.Succeeded)
            {
                return Deleted<string>();
            }
            else
            {
                var errors = string.Join(", ",
                    result.Errors.Select(e => e.Description));
                return BadRequest<string>(errors);
            }
        }

        public async Task<Response<string>> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
        {
            var RoleIsFound = await authorizationService.GetRoleByName(request.RoleName);
            if (RoleIsFound == null)
                return NotFound<string>();

            var user = await userManger.FindByEmailAsync(request.UserEmail);
            if (user == null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.UserNotFound]);

            var result = await userManger.AddToRoleAsync(user, RoleIsFound.Name);
            if (result.Succeeded)
            {

                return Updated<string>(RoleIsFound.Name);
            }
            else
            {
                var errors = string.Join(", ",
                    result.Errors.Select(e => e.Description));
                return BadRequest<string>(errors);
            }

        }

        public async Task<Response<string>> Handle(
      ResetUserRoleCommand request,
      CancellationToken cancellationToken)
        {
            var user = await userManger.FindByEmailAsync(request.UserEmail);
            if (user == null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.UserNotFound]);

            var currentRoles = await userManger.GetRolesAsync(user);

            if (currentRoles.Any())
            {
                var removeResult = await userManger.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    var errors = string.Join(", ", removeResult.Errors.Select(e => e.Description));
                    return BadRequest<string>(errors);
                }
            }
            if (!await userManger.IsInRoleAsync(user, "Student"))
            {
                await userManger.AddToRoleAsync(user, "Student");
            }

            return Updated<string>();
        }



        public async Task<Response<string>> Handle(AcceptInstructorRoleRequestCommand request, CancellationToken cancellationToken)
        {

            var roleRequest = await instructorProfilesService.GetById(request.RequestId);
            if (roleRequest is null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.Requestnotfound]);
            if (roleRequest.StatusOfInstructorProfile == StatusOfInstructorProfileEnum.Approved)
                return new Response<string>("Request already handled");
            var user = await userManger.FindByIdAsync(roleRequest.UserId.ToString());
            if (user is null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.UserNotFound]);
            if (!await userManger.IsInRoleAsync(user, "Instructor"))
            {
                await userManger.RemoveFromRolesAsync(user, await userManger.GetRolesAsync(user));
                await userManger.AddToRoleAsync(user, "Instructor");
            }
            roleRequest.StatusOfInstructorProfile = StatusOfInstructorProfileEnum.Approved;
            await instructorProfilesService.Update(roleRequest, null);
            await mediator.Publish(new InstructorRoleApprovedEvent(Guid.Parse(user.Id)), cancellationToken);
            return Success<string>("Instructor role approved successfully");
        }

        public async Task<Response<string>> Handle(
           RejectInstructorRoleRequestCommand request,
           CancellationToken cancellationToken)
        {
            var roleRequest = await instructorProfilesService.GetById(request.RequestId);
            if (roleRequest is null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.Requestnotfound]);

            if (roleRequest.StatusOfInstructorProfile == StatusOfInstructorProfileEnum.Rejected)
                return BadRequest<string>("Request already Rejected ");

            var user = await userManger.FindByIdAsync(roleRequest.UserId.ToString());
            if (user is null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.UserNotFound]);

            if (await userManger.IsInRoleAsync(user, "Instructor"))
            {
                await userManger.RemoveFromRoleAsync(user, "Instructor");
                await userManger.AddToRoleAsync(user, "Student");
            }

            roleRequest.StatusOfInstructorProfile = StatusOfInstructorProfileEnum.Rejected;

            await instructorProfilesService.Update(roleRequest, request.Reason);

            await mediator.Publish(new InstructorRoleRejectedEvent(Guid.Parse(user.Id), request.Reason));

            return Success<string>("Instructor role Rejected");
        }
    }
}
