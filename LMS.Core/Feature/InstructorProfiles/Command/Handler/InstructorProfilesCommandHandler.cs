using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.Authorization.Command.Handler;
using LMS.Core.Feature.InstructorProfiles.Command.Models;
using LMS.Core.Resources;
using LMS.Data_;
using LMS.Data_.Entities;
using LMS.Data_.Enum;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
namespace LMS.Core.Feature.InstructorProfiles.Command.Handler
{
    public class InstructorProfilesCommandHandler : ResponseHandler,
        IRequestHandler<AddInstructorProfileCommand, Response<string>>,
     IRequestHandler<UpdateInstructorProfileCommand, Response<string>>,
        IRequestHandler<CancelRequestInstructorProfileCommand, Response<string>>,
        IRequestHandler<DeleteInstructorProfileCommand, Response<string>>,

        INotificationHandler<InstructorRoleApprovedEvent>,
        INotificationHandler<InstructorRoleRejectedEvent>
    {
        private readonly IInstructorProfilesService instructorProfilesService;
        private readonly IMapper mapper;
        private readonly UserManager<Users> userManager;
        private readonly ILogger<IInstructorProfilesService> logger;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly RoleManager<Role> roleManager;
        private readonly ICurrentUserService currentUserService;

        public InstructorProfilesCommandHandler(IInstructorProfilesService instructorProfilesService, IMapper mapper, UserManager<Users> userManager, ILogger<IInstructorProfilesService> logger, IStringLocalizer<SharedResources> stringLocalizer, RoleManager<Role> roleManager, ICurrentUserService currentUserService) : base(stringLocalizer)
        {
            this.instructorProfilesService = instructorProfilesService;
            this.mapper = mapper;
            this.userManager = userManager;
            this.logger = logger;
            this.stringLocalizer = stringLocalizer;
            this.roleManager = roleManager;
            this.currentUserService = currentUserService;
        }
        public async Task<Response<string>> Handle(AddInstructorProfileCommand request, CancellationToken cancellationToken)
        {

            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return NotFound<string>();

            if (await instructorProfilesService.GetInstructorProfilesByUserId(user.Id) != null)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.InstructorprofileRequestalreadyexists]);
            var instructorProfile = mapper.Map<LMS.Data_.Entities.InstructorProfiles>(request);
            instructorProfile.UserId = user.Id;
            instructorProfile.StatusOfInstructorProfile = StatusOfInstructorProfileEnum.Pending;
            instructorProfile.CreatedAt = DateTime.UtcNow;


            var Result = await instructorProfilesService.Add(instructorProfile, request.ProfilePicture);
            if (Result == null)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedAdded]);

            return Created<string>(null);
        }

        public async Task Handle(
            InstructorRoleApprovedEvent notification,
            CancellationToken cancellationToken)
        {
            var instructorProfile = await instructorProfilesService
                .GetInstructorProfilesByUserId(notification.UserId.ToString());



            if (instructorProfile is null)
                return;

            if (instructorProfile.StatusOfInstructorProfile == StatusOfInstructorProfileEnum.Approved)
                return;

            instructorProfile.StatusOfInstructorProfile = StatusOfInstructorProfileEnum.Approved;

            await instructorProfilesService.Update(instructorProfile, null);
            var user = await userManager.FindByIdAsync(notification.UserId.ToString());

            if (await userManager.IsInRoleAsync(user, "Student"))
            {
                await userManager.RemoveFromRoleAsync(user, "Student");
            }

            if (!await userManager.IsInRoleAsync(user, "Instructor"))
            {
                await userManager.AddToRoleAsync(user, "Instructor");
            }
            logger.LogInformation(
                "Instructor profile approved for user {UserId}",
                notification.UserId);
        }
        public async Task Handle(
            InstructorRoleRejectedEvent notification,
            CancellationToken cancellationToken)
        {
            var instructorProfile = await instructorProfilesService
                .GetInstructorProfilesByUserId(notification.UserId.ToString());

            if (instructorProfile is null)
                return;

            if (instructorProfile.StatusOfInstructorProfile == StatusOfInstructorProfileEnum.Rejected)
                return;

            instructorProfile.StatusOfInstructorProfile = StatusOfInstructorProfileEnum.Rejected;

            await instructorProfilesService.Update(instructorProfile, notification.Reason);

            logger.LogInformation(
                "Instructor profile rejected for user {UserId}",
                notification.UserId);
        }

        public async Task<Response<string>> Handle(
       UpdateInstructorProfileCommand request,
       CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var instructorProfile = await instructorProfilesService.GetInstructorProfilesByUserId(userId);
            if (instructorProfile == null)
                return NotFound<string>();

            var user = await userManager.FindByIdAsync(instructorProfile.UserId.ToString());
            if (user == null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.UserNotFound]);

            if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
            {
                var emailExists = await userManager.FindByEmailAsync(request.Email);
                if (emailExists != null)
                    return BadRequest<string>(stringLocalizer[SharedResourcesKeys.EmailMustBeUniqueItIsFoundAlready]);

                user.Email = request.Email;
                await userManager.UpdateAsync(user);
            }

            mapper.Map(request, instructorProfile);
            instructorProfile.StatusOfInstructorProfile = StatusOfInstructorProfileEnum.Pending;


            var result = await instructorProfilesService.UpdateWithImage(instructorProfile, request.ProfilePicture);
            if (result == null)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedUpdated]);


            return Updated<string>();
        }

        public async Task<Response<string>> Handle(CancelRequestInstructorProfileCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            var instructorProfile = await instructorProfilesService.GetInstructorProfilesByUserId(userId);
            if (instructorProfile == null)
                return NotFound<string>();
            if (instructorProfile.StatusOfInstructorProfile != StatusOfInstructorProfileEnum.Pending)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.Onlypendingrequestscanbecancelled]);
            var result = await instructorProfilesService.Delete(instructorProfile.Id);
            if (!result)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedDeleted]);
            return Deleted<string>(stringLocalizer[SharedResourcesKeys.Deleted]);
        }

        public async Task<Response<string>> Handle(DeleteInstructorProfileCommand request, CancellationToken cancellationToken)
        {
            var instructorProfile = await instructorProfilesService.GetById(request.Id);
            if (instructorProfile == null)
                return NotFound<string>();
            var result = await instructorProfilesService.Delete(request.Id);

            if (!result)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedDeleted]);
            return Deleted<string>(stringLocalizer[SharedResourcesKeys.Deleted]);


        }


    }
}
