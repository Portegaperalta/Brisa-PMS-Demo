using BrisaPMS.Domain.Guests;
using BrisaPMS.Domain.Hotels;
using BrisaPMS.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrisaPMS.Persistence.Configurations;

public class GuestConfig : BaseEntityConfig<Guest>
{
    public override void Configure(EntityTypeBuilder<Guest> builder)
    {
        base.Configure(builder);

        builder.Property(g => g.Id)
               .IsRequired();

        builder.HasOne<Hotel>()
               .WithMany()
               .HasForeignKey(g => g.HotelId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(g => g.FirstName)
               .IsRequired()
               .HasMaxLength(250);

        builder.Property(g => g.LastName)
               .IsRequired()
               .HasMaxLength(250);
        
        builder.Property(g => g.DocumentType)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(20);
         
        builder.Property(g => g.DocumentNumber)
               .IsRequired()
               .HasMaxLength(250);

        builder.Property(g => g.Country)
               .HasMaxLength(100);
        
        builder.Property(g => g.Rnc)
               .HasConversion(
                      v => v != null ? v.Value : null,
                      v => v != null ? new Rnc(v) : null)
               .HasMaxLength(11);
        
        builder.Property(g => g.Email)
               .HasConversion(v => v.Value, v => new Email(v))
               .HasMaxLength(254);
        
        builder.Property(g => g.PhoneNumber)
               .HasConversion(v => v.Value, v => new PhoneNumber(v))
               .HasMaxLength(25);
        
        builder.Property(g => g.PreferredCurrency)
               .HasConversion<string>()
               .HasMaxLength(3);

        builder.Property(g => g.PreferredLanguage)
               .HasMaxLength(50);

        builder.Property(g => g.IsVip)
               .IsRequired();

        builder.Property(g => g.IsBlackListed)
               .IsRequired();
        
        builder.Property(g => g.BlackListedReason)
               .HasMaxLength(500);

        builder.Property(g => g.Notes)
               .HasMaxLength(500);
    }
}