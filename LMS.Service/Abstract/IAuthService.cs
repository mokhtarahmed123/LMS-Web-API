using LMS.Data_.Entities;
using LMS.Data_.Helper;

namespace LMS.Service.Abstract
{
    public interface IAuthService
    {
        public Task<JWTAuthResponse> GenerateJWToken(Users users);

        public Task SaveRefreshToken(RefreshToken refreshToken, Users user);
        public Task<JWTAuthResponse> GetRefreshToken(string refreshTokenString, string Token);
        Task RevokeRefreshToken(string UserId);
        public Task<string> SignUp(Users users, string Password);
        public Task<string> ConfirmEmail(string UserId, string Code);
        public Task<string> SendResetPasswordCode(string email);
        public Task<string> ResetPasswordCode(string email, string Password);
        public Task<string> ConfirmResetPassword(string Code, string Email);
        public Task<string> ValidateToken(string Token);


    }
}
