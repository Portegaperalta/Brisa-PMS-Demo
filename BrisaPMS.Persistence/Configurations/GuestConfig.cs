using BrisaPMS.Domain.Guests;
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

        builder.Property(g => g.HotelId)
               .IsRequired();
        
        builder.Property(g => g.DocumentType)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(20);
         
        builder.Property(g => g.DocumentNumber)
               .IsRequired()
               .HasMaxLength(250);

        builder.Property(g => g.Country)
               .HasDefaultValue(null)
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
               .HasDefaultValue(null)
               .HasMaxLength(50);

        builder.Property(g => g.IsVip)
               .IsRequired()
               .HasDefaultValue(false);
        
        builder.Property(g => g.IsBlackListed)
               .IsRequired()
               .HasDefaultValue(false);
        
        builder.Property(g => g.BlackListedReason)
               .HasDefaultValue(null)
               .HasMaxLength(500);

        builder.Property(g => g.Notes)
               .HasDefaultValue(null)
               .HasMaxLength(500);
    }
}