using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.ApplicationUser.Command.Models.AuthModel;
using LMS.Core.Feature.Emails.Query.Models;
using LMS.Core.Resources;
using LMS.Data_;
using LMS.Data_.Entities;
using LMS.Data_.Helper;
using LMS.Infrastructure.Caching.Redis;
using LMS.Service.Abstract;
using LMS.Service.EmailServices;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System.IdentityModel.Tokens.Jwt;
namespace LMS.Core.Feature.ApplicationUser.Command.Handler
{
    public class AuthCommandHandler : ResponseHandler,
        IRequestHandler<SignUpUserCommand, Response<string>>,
        IRequestHandler<LoginCommand, Response<JWTAuthResponse>>,
        IRequestHandler<LogOutCommand, Response<string>>
        , IRequestHandler<ConfirmEmailQuery, Response<string>>,
        IRequestHandler<SendResetPasswordCommand, Response<string>>,
        IRequestHandler<ResetPasswordCommand, Response<string>>,
        IRequestHandler<RefreshTokenCommand, Response<JWTAuthResponse>>
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
        private readonly ICurrentUserService common;
        private readonly ICacheService cacheService;

        public AuthCommandHandler(IMapper mapper, UserManager<Users> userManager, RoleManager<Role> roleManager, IStringLocalizer<SharedResources> stringLocalizer, IUserService userService, IUserCoursesService userCoursesService, SignInManager<Users> signInManager, IAuthService authService, IHttpContextAccessor httpContextAccessor, IEmailService emailService, ICurrentUserService common, ICacheService cacheService) : base(stringLocalizer)
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
            this.common = common;
            this.cacheService = cacheService;
        }
        public async Task<Response<string>> Handle(SignUpUserCommand request, CancellationToken cancellationToken)
        {
            var user = mapper.Map<Users>(request);

            var result = await authService.SignUp(user, request.Password);

            switch (result)
            {
                case "UserWithEmailIsAlreadyFound":
                    return BadRequest<string>(stringLocalizer[SharedResourcesKeys.UserWithEmailIsAlreadyFound]);

                case "Failed":
                    return BadRequest<string>(stringLocalizer[SharedResourcesKeys.EmailSendFailed]);

                case "Success":
                    return Created<string>($"{stringLocalizer[SharedResourcesKeys.Created]} — Now Log In to Get Started!");

                default:
                    return BadRequest<string>(result);
            }

        }

        public async Task<Response<JWTAuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null) return NotFound<JWTAuthResponse>(stringLocalizer[SharedResourcesKeys.EmailNotFound]);

            var Password = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!user.EmailConfirmed)
                return BadRequest<JWTAuthResponse>(stringLocalizer[SharedResourcesKeys.NotConfirmEmail]);

            if (!Password.Succeeded) return BadRequest<JWTAuthResponse>(stringLocalizer[SharedResourcesKeys.PasswordIsWrong]);
            var Result = await authService.GenerateJWToken(user);
            return Success<JWTAuthResponse>(Result);
        }

        public async Task<Response<string>> Handle(LogOutCommand request, CancellationToken cancellationToken)
        {
            var userId = common.UserIdFromJWT();

            if (string.IsNullOrEmpty(userId))
                return NotFound<string>("User not found");


            var token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
                .ToString().Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {

                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var expiry = jwtToken.ValidTo - DateTime.UtcNow;

                if (expiry > TimeSpan.Zero)
                    await cacheService.SetDataAsync($"blacklist:{token}", "true", expiry);
            }

            await authService.RevokeRefreshToken(userId);

            return Success("Logged out successfully");
        }

        public async Task<Response<string>> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            var Confirm = await authService.ConfirmEmail(request.UserId, request.Code);
            switch (Confirm)
            {
                case "UserIdOrCodeNull":
                    return BadRequest<string>(stringLocalizer[SharedResourcesKeys.NotEmpty]);
                case "UserNotFound":
                    return NotFound<string>(stringLocalizer[SharedResourcesKeys.UserNotFound]);
                case "Failed":
                    return BadRequest<string>("Failed");
                case "Confirmed":
                    return Success<string>("Confirmed");
                default:
                    return BadRequest<string>("");
            }
        }
        public async Task<Response<string>> Handle(SendResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await authService.SendResetPasswordCode(request.Email);

            return result switch
            {
                "UserNotFound" => NotFound<string>(stringLocalizer[SharedResourcesKeys.UserNotFound]),
                "CodeIsWrong" => BadRequest<string>(stringLocalizer[SharedResourcesKeys.CodeIsWrong]),
                "Success" => Success<string>(stringLocalizer[SharedResourcesKeys.CodeSent]),
                _ => BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedUpdated])
            };
        }

        public async Task<Response<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.Password.Length < 8 || !request.Password.Any(char.IsUpper) ||
             !request.Password.Any(char.IsLower) ||
            !request.Password.Any(char.IsDigit) ||
             !request.Password.Any(char.IsSymbol) && !request.Password.Any(char.IsPunctuation))
                return BadRequest<string>("PasswordTooWeak");


            var result = await authService.ResetPasswordCode(request.Email, request.Password);

            return result switch
            {
                "UserNotFound" => NotFound<string>(stringLocalizer[SharedResourcesKeys.UserNotFound]),
                "Failed" => BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedUpdated]),
                "Success" => Success<string>(" Reset Password Successfully Login  Now    "),
                _ => BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedUpdated])
            };

        }

        public async Task<Response<JWTAuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var Result = await authService.GetRefreshToken(request.RefreshToken, request.Token);
            return Success<JWTAuthResponse>(Result);
        }




    }
}
