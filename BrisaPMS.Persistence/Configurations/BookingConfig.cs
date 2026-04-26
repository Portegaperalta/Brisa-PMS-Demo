using BrisaPMS.Domain.Bookings;
using BrisaPMS.Domain.Guests;
using BrisaPMS.Domain.Hotels;
using BrisaPMS.Domain.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrisaPMS.Persistence.Configurations;

public class BookingConfig : BaseEntityConfig<Booking>
{
    public override void Configure(EntityTypeBuilder<Booking> builder)
    {
        base.Configure(builder); // audit fields from base entity config   
           
        builder.Property(b => b.Id)
               .IsRequired();
        
        builder.HasOne<Hotel>()
               .WithMany()
               .HasForeignKey(b => b.HotelId)
               .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne<Room>()
               .WithMany()
               .HasForeignKey(b => b.RoomId)
               .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne<Guest>()
               .WithMany()
               .HasForeignKey(b => b.GuestId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(b => b.Source)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(200);

        builder.OwnsOne(b => b.GuestCount, guestCount =>
        {
               guestCount.Property(g => g.NumberOfAdults)
                      .IsRequired()
                      .HasMaxLength(10);

               guestCount.Property(g => g.NumberOfChildren)
                      .IsRequired()
                      .HasMaxLength(10)
                      .HasDefaultValue(0);
        });

        builder.OwnsOne(p => p.CheckInOutTimes, checkInOutTimes =>
        {
               checkInOutTimes.Property(c => c.CheckInTime)
                      .IsRequired();

               checkInOutTimes.Property(c => c.CheckOutTime)
                      .IsRequired();
        });

        builder.Property(b => b.SpecialRequests)
               .HasMaxLength(500)
               .HasDefaultValue(null);

        builder.Property(b => b.Status)
               .IsRequired()
               .HasConversion<string>();

        builder.Property(b => b.CancellationReason)
               .HasMaxLength(255);
        
        builder.Property(b => b.TotalPrice)
               .IsRequired()
               .HasColumnType("decimal(10,2)");

        builder.Property(b => b.DiscountId);
    }
}