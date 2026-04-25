using BrisaPMS.Application.Contracts.Services;
using BrisaPMS.Domain.Amenities;
using BrisaPMS.Domain.Bookings;
using BrisaPMS.Domain.Companies;
using BrisaPMS.Domain.Guests;
using BrisaPMS.Domain.Hotels;
using BrisaPMS.Domain.HouseKeeping;
using BrisaPMS.Domain.Rooms;
using BrisaPMS.Domain.RoomTypes;
using BrisaPMS.Domain.Shared.Abstractions;
using BrisaPMS.Domain.Stays;
using BrisaPMS.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace BrisaPMS.Persistence
{
    public class BrisaPmsDbContext : DbContext
    {
        private readonly ICurrentUserService _currentUser;
        
        public BrisaPmsDbContext(DbContextOptions<BrisaPmsDbContext> options, ICurrentUserService currentUser) 
            : base(options)
        {
            _currentUser = currentUser;
        }

        protected BrisaPmsDbContext()
        {
        }
        
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = _currentUser.UserId;
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedBy = _currentUser.UserId;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }
            
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BrisaPmsDbContext).Assembly);
        }

        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<HouseKeepingTask> HouseKeepingTasks { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Stay> Stays { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
