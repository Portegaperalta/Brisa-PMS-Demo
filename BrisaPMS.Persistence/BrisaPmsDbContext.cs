using Microsoft.EntityFrameworkCore;

namespace BrisaPMS.Persistence
{
    public class BrisaPmsDbContext : DbContext
    {
        public BrisaPmsDbContext(DbContextOptions<BrisaPmsDbContext> options) : base(options)
        {
        }

        protected BrisaPmsDbContext()
        {
        }
    }
}
