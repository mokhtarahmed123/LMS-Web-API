using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.ApplicationUser.Command.Models.ProfileModel;
using LMS.Core.Feature.ApplicationUser.Query.Result.ProfileResult;
using LMS.Core.Resources;
using LMS.Data_;
using LMS.Data_.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.ApplicationUser.Query.Handler
{
    public class ProfileQueryHandler : ResponseHandler, IRequestHandler<MyProfileQuery, Response<MyProfileResult>>
    {
        private readonly UserManager<Users> userManager;
        private readonly SignInManager<Users> signInManager;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly RoleManager<Role> roleManager;
        private readonly ICurrentUserService common;

        public ProfileQueryHandler(UserManager<Users> userManager, SignInManager<Users> signInManager, IMapper mapper, IHttpContextAccessor httpContextAccessor, IStringLocalizer<SharedResources> stringLocalizer, RoleManager<Role> roleManager, ICurrentUserService common) : base(stringLocalizer)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.stringLocalizer = stringLocalizer;
            this.roleManager = roleManager;
            this.common = common;
        }


        public async Task<Response<MyProfileResult>> Handle(MyProfileQuery request, CancellationToken cancellationToken)
        {
            var Id = common.UserId;
            var IsFound = await userManager.FindByIdAsync(Id);
            if (IsFound == null) return NotFound<MyProfileResult>(stringLocalizer[SharedResourcesKeys.UserNotFound]);
            var Mapped = mapper.Map<MyProfileResult>(IsFound);
            var roles = await userManager.GetRolesAsync(IsFound);
            var roleName = roles.FirstOrDefault();
            Mapped.RoleName = roleName;
            return Success(Mapped);
        }
    }
}
