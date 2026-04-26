using BrisaPMS.Domain.Bookings;
using BrisaPMS.Domain.Guests;
using BrisaPMS.Domain.Stays;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrisaPMS.Persistence.Configurations;

public class StayConfig : BaseEntityConfig<Stay>
{
    public override void Configure(EntityTypeBuilder<Stay> builder)
    {
        base.Configure(builder);

        builder.Property(s => s.Id)
               .IsRequired();

        builder.HasOne<Guest>()
               .WithMany()
               .HasForeignKey(s => s.GuestId)
               .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne<Booking>()
               .WithMany()
               .HasForeignKey(s => s.BookingId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(s => s.TimeInterval, timeInterval =>
        {
            timeInterval.Property(t => t.ActualCheckIn)
                        .IsRequired();

            timeInterval.Property(t => t.ActualCheckOut);
        });

        builder.Property(s => s.NightCount)
               .IsRequired();

        builder.Property(s => s.Status)
               .IsRequired()
               .HasConversion<string>();
    }
}