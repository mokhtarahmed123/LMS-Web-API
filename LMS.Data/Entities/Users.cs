using EntityFrameworkCore.EncryptColumn.Attribute;
using Microsoft.AspNetCore.Identity;

namespace LMS.Data_.Entities
{
    public class Users : IdentityUser
    {


        public List<RefreshToken>? RefreshTokens { get; set; } = new();

        [EncryptColumn]
        public string? Code { get; set; }

    }
}
