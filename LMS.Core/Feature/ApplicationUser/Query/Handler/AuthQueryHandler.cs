using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.ApplicationUser.Query.Models.AuthModel;
using LMS.Core.Resources;
using LMS.Data_.Entities;
using LMS.Service.Abstract;
using LMS.Service.EmailServices;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.ApplicationUser.Query.Handler
{
    public class AuthQueryHandler : ResponseHandler, IRequestHandler<ConfirmResetPasswordQuery, Response<string>>
    {
        private readonly IMapper mapper;
        private readonly UserManager<Users> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly IUserService userService;
        private readonly IUserCoursesService userCoursesService;
        private readonly SignInManager<Users> signInManager;
        private readonly IAuthService authService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IEmailService emailService;

        public AuthQueryHandler(IMapper mapper, UserManager<Users> userManager, RoleManager<Role> roleManager, IStringLocalizer<SharedResources> stringLocalizer, IUserService userService, IUserCoursesService userCoursesService, SignInManager<Users> signInManager, IAuthService authService, IHttpContextAccessor httpContextAccessor, IEmailService emailService) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.stringLocalizer = stringLocalizer;
            this.userService = userService;
            this.userCoursesService = userCoursesService;
            this.signInManager = signInManager;
            this.authService = authService;
            this.httpContextAccessor = httpContextAccessor;
            this.emailService = emailService;
        }




        public async Task<Response<string>> Handle(ConfirmResetPasswordQuery request, CancellationToken cancellationToken)
        {
            var result = await authService.ConfirmResetPassword(request.Code, request.Email);

            return result switch
            {
                "UserNotFound" => NotFound<string>(stringLocalizer[SharedResourcesKeys.UserNotFound]),
                "ErrorInUpdating" => BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedUpdated]),
                "FailedToSendEmail" => BadRequest<string>(stringLocalizer[SharedResourcesKeys.SendEmailFailed]),
                "Success" => Success<string>("Correct Code "),
                _ => BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedUpdated])
            };
        }
    }
}
