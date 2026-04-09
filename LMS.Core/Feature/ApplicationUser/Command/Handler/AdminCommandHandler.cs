using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.ApplicationUser.Command.Models.AdminModel;
using LMS.Core.Resources;
using LMS.Data_.Entities;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.ApplicationRole.Command.Handler
{
    public class AdminCommandHandler : ResponseHandler,
        IRequestHandler<DeleteUserCommand, Response<string>>
    {
        private readonly IMapper mapper;
        private readonly UserManager<Users> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly IUserService userService;
        private readonly IUserCoursesService userCoursesService;

        public AdminCommandHandler(IMapper mapper, UserManager<Users> userManager, RoleManager<Role> roleManager, IStringLocalizer<SharedResources> stringLocalizer, IUserService userService, IUserCoursesService userCoursesService) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.stringLocalizer = stringLocalizer;
            this.userService = userService;
            this.userCoursesService = userCoursesService;
        }

        public async Task<Response<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UserId))
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.Required]);

            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.UserNotFound]);

            var IfUserSubscribeInAnyCourse = await userCoursesService.GetUserCoursesByUserIdAsync(request.UserId);
            if (IfUserSubscribeInAnyCourse != null)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.YouCannotDeleteAccountYouSubscribeInCourse]);


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

            return Deleted<string>();
        }



    }
}
