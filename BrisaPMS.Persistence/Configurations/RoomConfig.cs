using BrisaPMS.Domain.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrisaPMS.Persistence.Configurations;

public class RoomConfig : BaseEntityConfig<Room>
{
    public override void Configure(EntityTypeBuilder<Room> builder)
    {
        base.Configure(builder);

        builder.Property(r => r.Id)
               .IsRequired();
        
        builder.Property(r => r.HotelId)
               .IsRequired();
        
        builder.Property(r => r.Number)
               .IsRequired()
               .HasMaxLength(100);
        
        builder.Property(r => r.Floor)
               .IsRequired()
               .HasMaxLength(200);
        
        builder.Property(r => r.AvailabilityStatus)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(11);
        
        builder.Property(r => r.HygieneStatus)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(11);

        builder.Property(r => r.LastCleanedAt)
               .HasDefaultValue(null);
        
        builder.Property(r => r.LastCleanedBy)
               .HasDefaultValue(null);

        builder.Property(r => r.NeedsRestocking)
               .HasDefaultValue(false);
    }
}