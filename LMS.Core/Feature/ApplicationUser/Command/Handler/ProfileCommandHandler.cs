using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.ApplicationUser.Command.Models.ProfileModel;
using LMS.Core.Resources;
using LMS.Data_;
using LMS.Data_.Entities;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.ApplicationUser.Command.Handler
{
    public class ProfileCommandHandler : ResponseHandler,
        IRequestHandler<ChangePasswordCommand, Response<string>>,
          IRequestHandler<UpdateMyProfileCommand, Response<string>>,
          IRequestHandler<DeleteMyProfileCommand, Response<string>>
    {

        private readonly IMapper mapper;
        private readonly UserManager<Users> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly IUserService userService;
        private readonly IUserCoursesService userCoursesService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ISubscriptionsService subscriptionsService;
        private readonly IUserCoursesService userCourses;
        private readonly IInstructorProfilesService instructorProfilesService;
        private readonly SignInManager<Users> signInManager;
        private readonly ICurrentUserService common;

        public ProfileCommandHandler(IMapper mapper, UserManager<Users> userManager,
            RoleManager<Role> roleManager, IStringLocalizer<SharedResources> stringLocalizer,
            IUserService userService, IUserCoursesService userCoursesService, IHttpContextAccessor httpContextAccessor,
            ISubscriptionsService subscriptionsService, IUserCoursesService userCourses, IInstructorProfilesService instructorProfilesService, SignInManager<Users> signInManager, ICurrentUserService common) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.stringLocalizer = stringLocalizer;
            this.userService = userService;
            this.userCoursesService = userCoursesService;
            this.httpContextAccessor = httpContextAccessor;
            this.subscriptionsService = subscriptionsService;
            this.userCourses = userCourses;
            this.instructorProfilesService = instructorProfilesService;
            this.signInManager = signInManager;
            this.common = common;
        }

        public async Task<Response<string>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var Id = common.UserId;
            var User = await userManager.FindByIdAsync(Id);
            if (User == null) return NotFound<string>(stringLocalizer[SharedResourcesKeys.UserNotFound]);
            var PasswordIsCorrect = await userManager.CheckPasswordAsync(User, request.OldPassword);
            if (!PasswordIsCorrect) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.PasswordIsWrong]);

            var Result = await userManager.ChangePasswordAsync(User, request.OldPassword, request.NewPassword);
            if (!Result.Succeeded) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedUpdated]);
            return Updated<string>("Password changed successfully. Please log in with your new password.");
        }
        public async Task<Response<string>> Handle(UpdateMyProfileCommand request, CancellationToken cancellationToken)
        {
            var Id = common.UserId;
            var user = await userManager.FindByIdAsync(Id);

            if (user == null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.UserNotFound]);

            var emailIsRepeated = await userService.EmailNotRepeatedWhenUpdated(Id, request.Email);

            if (!emailIsRepeated)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.UserWithEmailIsAlreadyFound]);

            mapper.Map(request, user);

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest<string>(errors);
            }

            return Updated<string>();
        }

        public async Task<Response<string>> Handle(DeleteMyProfileCommand request, CancellationToken cancellationToken)
        {
            var userId = common.UserId; ;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.UserNotFound]);

            var enrolledCourses = await userCourses.GetUserCoursesByUserIdAsync(userId);
            if (enrolledCourses.Any())
            {
                await userCourses.DeleteAllUserCourses(userId);
            }
            var activeSubscriptions = await subscriptionsService.GetAllSubscriptionsAreActivesByUserId(userId);
            if (activeSubscriptions.Any())
            {
                await subscriptionsService.DeleteAllUserSubscription(userId);
            }

            var Instructor = await instructorProfilesService.GetInstructorProfilesByUserId(userId);
            if (Instructor != null) await instructorProfilesService.Delete(Instructor.Id);



            var roles = await userManager.GetRolesAsync(user);
            if (roles.Any())
            {
                await userManager.RemoveFromRolesAsync(user, roles);
            }


            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest<string>(errors);
            }
            await signInManager.SignOutAsync();
            return Deleted<string>();
        }


    }
}
