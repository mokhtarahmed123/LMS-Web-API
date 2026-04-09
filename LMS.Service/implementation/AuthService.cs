using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using LMS.Data_.Entities;
using LMS.Data_.Helper;
using LMS.Infrastructure.Context;
using LMS.Infrastructure.Migrations;
using LMS.Service.Abstract;
using LMS.Service.EmailServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LMS.Service.implementation
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration configuration;
        private readonly RoleManager<Role> roleManager;
        private readonly UserManager<Users> userManager;
        private readonly ConcurrentDictionary<string, Data_.Entities.RefreshToken> useRefreshToken;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IEmailService emailService;
        private readonly AppDbContext appDbContext;
        private readonly IEncryptionProvider encryptionProvider;

        public AuthService(IConfiguration configuration, RoleManager<Role> roleManager, UserManager<Users> userManager, ConcurrentDictionary<string, Data_.Entities.RefreshToken> UseRefreshToken, IHttpContextAccessor httpContextAccessor, IEmailService emailService, AppDbContext appDbContext)
        {
            this.configuration = configuration;
            this.roleManager = roleManager;
            this.userManager = userManager;
            useRefreshToken = UseRefreshToken;
            this.httpContextAccessor = httpContextAccessor;
            this.emailService = emailService;
            this.appDbContext = appDbContext;
            this.encryptionProvider = new GenerateEncryptionProvider("paoiebfec64f99d943d983ed99fabcd1");

        }

        public async Task<string> ConfirmEmail(string UserId, string Code)
        {
            if (UserId == null || Code == null) return "UserIdOrCodeNull";

            var User = await userManager.FindByIdAsync(UserId);
            if (User == null) return "UserNotFound";
            var confirmEmail = await userManager.ConfirmEmailAsync(User, Code);
            if (!confirmEmail.Succeeded) return "Failed";
            return "Confirmed";
        }

        public async Task<JWTAuthResponse> GenerateJWToken(Users user)
        {
            int RefreshTokenExpiredDate = int.Parse(configuration["JWT:RefreshTokenExpiredDate"]);
            var Token = await GetJwtToken(user);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(Token);
            var RefreshToken = new Data_.Entities.RefreshToken
            {
                CreatedAt = DateTime.UtcNow,

                UserName = user.UserName,
                ExpireOn = DateTime.UtcNow.AddDays(RefreshTokenExpiredDate),
                refreshToken = GenerateRefreshToken()
            };
            await SaveRefreshToken(RefreshToken, user);
            useRefreshToken.AddOrUpdate(RefreshToken.refreshToken, RefreshToken, (s, t) => RefreshToken);
            return new JWTAuthResponse
            {
                RefreshToken = RefreshToken,
                Token = tokenString
            };
        }
        public async Task<JWTAuthResponse> GetRefreshToken(string refreshTokenString, string accessToken)
        {

            var token = ReadJWToken(accessToken);
            if (token is null || token.Header.Alg != SecurityAlgorithms.HmacSha256)
                throw new SecurityTokenException("Invalid token.");

            var userId = token.Claims
                .FirstOrDefault(x => x.Type == nameof(UserClaimModel.Id))?.Value;

            if (string.IsNullOrEmpty(userId))
                throw new SecurityTokenException("Invalid token claims.");

            var user = await userManager.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(x => x.Id == userId)
                ?? throw new KeyNotFoundException("User not found.");

            var refreshToken = user.RefreshTokens
                .FirstOrDefault(x => x.refreshToken == refreshTokenString)
                ?? throw new SecurityTokenException("Refresh token not found.");

            if (!refreshToken.IsActive)
                throw new SecurityTokenException(
                    refreshToken.RevokedOn != null
                        ? "Refresh token has been revoked."
                        : "Refresh token has expired."
                );

            refreshToken.RevokedOn = DateTime.UtcNow;
            await userManager.UpdateAsync(user);

            return await GenerateJWToken(user);
        }

        private async Task<JwtSecurityToken> GetJwtToken(Users user)
        {
            var roleName = await rolename(user);

            var Claims = new List<Claim>
            {
                new Claim(nameof(UserClaimModel.Id),user.Id),
                new Claim(nameof(UserClaimModel.Email),user.Email),
                new Claim(nameof(UserClaimModel.roleName),roleName),
                new Claim(nameof(UserClaimModel.UserName),user.UserName)
            };
            var secretKey = configuration["JWT:SecretKey"];
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var Sign = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            int AccessTokenExpiredDate = int.Parse(configuration["JWT:AccessTokenExpiredDate"]);
            int RefreshTokenExpiredDate = int.Parse(configuration["JWT:RefreshTokenExpiredDate"]);

            var Token = new JwtSecurityToken
                (
                issuer: configuration["JWT:IssuerIP"],
                audience: configuration["JWT:AudienceIP"],
                claims: Claims,
                expires: DateTime.UtcNow.AddDays(AccessTokenExpiredDate),
                signingCredentials: Sign

                );

            return Token;
        }

        private async Task<string> rolename(Users user)
        {
            return (await userManager.GetRolesAsync(user)).FirstOrDefault();
        }

        public async Task<string> SendResetPasswordCode(string email)
        {

            await using var transaction = await appDbContext.Database.BeginTransactionAsync();
            try
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user == null) return "UserNotFound";

                var codeBytes = new byte[4];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(codeBytes);
                }
                int codeInt = BitConverter.ToInt32(codeBytes, 0) & 0x7FFFFFFF;
                string code = (codeInt % 1000000).ToString("D6");


                user.Code = code;

                var updateResult = await userManager.UpdateAsync(user);
                if (!updateResult.Succeeded) return "ErrorInUpdating";

                await transaction.CommitAsync();


                var emailBody = $"<h3>Hello {user.UserName}!</h3>" +
                                $"<p>Your password reset code is: <strong>{user.Code}</strong></p>" +
                                $"<p>Please use this code to reset your password.</p>";

                var emailResult = await emailService.SendEmailAsync(user.Email, emailBody, "Reset Password");

                if (emailResult != "Success")
                    return "FailedToSendEmail";

                return "Success";
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return "Failed";
            }
        }

        public async Task RevokeRefreshToken(string userId)
        {
            var user = await userManager.Users
                  .Include(u => u.RefreshTokens)
                  .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return;

            foreach (var token in user.RefreshTokens.Where(t => t.RevokedOn == null && !t.IsExpired))
            {
                token.RevokedOn = DateTime.UtcNow;
                token.ExpireOn = DateTime.UtcNow;

            }

            await userManager.UpdateAsync(user);
        }

        public async Task SaveRefreshToken(Data_.Entities.RefreshToken refreshToken, Users user)
        {

            user.RefreshTokens.Add(refreshToken);

            await userManager.UpdateAsync(user);
        }

        public async Task<string> SignUp(Users user, string password)
        {
            var trans = await appDbContext.Database.BeginTransactionAsync();
            try
            {
                var userWithEmail = await userManager.FindByEmailAsync(user.Email);
                if (userWithEmail != null)
                    return "UserWithEmailIsAlreadyFound";

                var createResult = await userManager.CreateAsync(user, password);
                if (!createResult.Succeeded)
                {
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    return errors;
                }

                var addRoleResult = await userManager.AddToRoleAsync(user, "Student");
                if (!addRoleResult.Succeeded)
                {
                    var errors = string.Join(", ", addRoleResult.Errors.Select(e => e.Description));
                    return errors;
                }

                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var codeEncoded = Uri.EscapeDataString(code);

                var request = httpContextAccessor.HttpContext.Request;
                var host = request.Host.HasValue ? request.Host.Value : "localhost";
                var link = $"{request.Scheme}://{host}/api/Auth/ConfirmEmail?userId={user.Id}&code={codeEncoded}";

                var emailBody = $"<h3>Welcome {user.UserName}!</h3>" +
                                $"<p>Please confirm your email by clicking the link below:</p>" +
                                $"<a href='{link}'>Confirm Email</a>";

                var result = await emailService.SendEmailAsync(user.Email, emailBody, "Confirm your email");

                if (result != "Success")
                    return "Failed";
                await trans.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                trans.RollbackAsync();
                return "Failed";
            }

        }

        private string GenerateRefreshToken()
        {
            var random = new Byte[32];
            using var genrator = RandomNumberGenerator.Create();
            genrator.GetBytes(random);
            return Convert.ToBase64String(random);
        }

        public async Task<string> ConfirmResetPassword(string Code, string Email)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user == null) return "UserNotFound";

            var usercode = user.Code;
            if (usercode != Code) return "CodeIsWrong";
            return "Success";
        }

        public async Task<string> ResetPasswordCode(string Email, string Password)
        {

            using var transaction = await appDbContext.Database.BeginTransactionAsync();
            try
            {
                var user = await userManager.FindByEmailAsync(Email);
                if (user == null) return "UserNotFound";
                await userManager.RemovePasswordAsync(user);
                var Result = await userManager.AddPasswordAsync(user, Password);
                await transaction.CommitAsync();
                return "Success";


            }
            catch (Exception ex)
            {


                await transaction.RollbackAsync();
                return "Failed";

            }
        }


        private JwtSecurityToken ReadJWToken(string JWToken)
        {
            if (string.IsNullOrEmpty(JWToken))
                throw new ArgumentNullException(nameof(JWToken));
            var Handler = new JwtSecurityTokenHandler();
            return Handler.ReadJwtToken(JWToken);
        }

        public async Task<string> ValidateToken(string Token)
        {
            var Handler = new JwtSecurityTokenHandler();

            var secretKey = configuration["JWT:SecretKey"];
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["JWT:IssuerIP"],

                ValidateAudience = true,
                ValidAudience = configuration["JWT:AudienceIP"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                RoleClaimType = "roleName",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            var validtor = Handler.ValidateToken(Token, parameters, out SecurityToken validatedToken);
            try
            {
                if (validtor == null) throw new SecurityTokenException("InvalidToken");
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
