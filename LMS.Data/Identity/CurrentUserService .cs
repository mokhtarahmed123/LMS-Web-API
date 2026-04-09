using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LMS.Data_
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

        public string UserId => GetClaim("Id") ?? throw new Exception("User Id claim not found");
        public string? Email => GetClaim(ClaimTypes.Email);
        public IEnumerable<string> Roles => User?.FindAll(ClaimTypes.Role).Select(r => r.Value) ?? Enumerable.Empty<string>();

        public string? GetClaim(string claimType)
        {
            return User?.FindFirst(claimType)?.Value;
        }

        public string UserIdFromJWT()
        {
            var user = httpContextAccessor.HttpContext?.User;
            var claim = user?.FindFirst("Id")?.Value;

            if (string.IsNullOrEmpty(claim))
                throw new Exception("User Id claim not found");

            return claim;

        }

        public string? UserIdFromJWTWithNull()
        {
            var user = httpContextAccessor.HttpContext?.User;
            var claim = user?.FindFirst("Id")?.Value;

            return claim;
        }
    }
}
