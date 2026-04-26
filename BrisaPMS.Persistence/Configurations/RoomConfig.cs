using BrisaPMS.Domain.Hotels;
using BrisaPMS.Domain.Rooms;
using BrisaPMS.Domain.RoomTypes;
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

        builder.HasOne<RoomType>()
               .WithMany()
               .HasForeignKey(r => r.RoomTypeId)
               .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne<Hotel>()
               .WithMany()
               .HasForeignKey(r => r.HotelId)
               .OnDelete(DeleteBehavior.Restrict);
        
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

        builder.Property(r => r.LastCleanedAt);

        builder.Property(r => r.LastCleanedBy);

        builder.Property(r => r.NeedsRestocking);
    }
}