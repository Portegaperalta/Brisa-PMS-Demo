using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BrisaPMS.Identity
{
    public class IdentityAppDbContext : IdentityDbContext<AppUser>
    {
        public IdentityAppDbContext(DbContextOptions<IdentityAppDbContext> options)
            : base(options)
        {
        }
    }
}