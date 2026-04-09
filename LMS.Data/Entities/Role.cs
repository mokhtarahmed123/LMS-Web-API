using Microsoft.AspNetCore.Identity;

namespace LMS.Data_.Entities
{
    public class Role : IdentityRole
    {
        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; } = new List<IdentityUserRole<string>>();

    }
}
