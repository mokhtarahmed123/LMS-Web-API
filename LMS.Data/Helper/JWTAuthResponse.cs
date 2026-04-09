using LMS.Data_.Entities;

namespace LMS.Data_.Helper
{
    public class JWTAuthResponse
    {
        public string Token { get; set; }
        public RefreshToken RefreshToken { get; set; }

    }
}
