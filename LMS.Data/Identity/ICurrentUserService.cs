namespace LMS.Data_
{
    public interface ICurrentUserService
    {
        public string? UserIdFromJWT();
        string UserId { get; }
        string? Email { get; }
        IEnumerable<string> Roles { get; }
        string? GetClaim(string claimType);

        public string? UserIdFromJWTWithNull();
    }
}
