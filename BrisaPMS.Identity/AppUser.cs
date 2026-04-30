using Microsoft.AspNetCore.Identity;

namespace BrisaPMS.Identity
{
    public class AppUser : IdentityUser
    {
        public Guid DomainUserId { get; set; }
    }
}
