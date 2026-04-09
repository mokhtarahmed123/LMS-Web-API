using Microsoft.EntityFrameworkCore;

namespace LMS.Data_.Entities
{
    [Owned]
    public class RefreshToken
    {
        public string refreshToken { get; set; }

        public string UserName { get; set; }

        public DateTime ExpireOn { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? RevokedOn { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpireOn;

        public bool IsActive => RevokedOn == null && !IsExpired;
    }
}