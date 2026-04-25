using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Hotels;
using BrisaPMS.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrisaPMS.Persistence.Configurations;

public class HotelConfig : BaseEntityConfig<Hotel>
{
    public override void Configure(EntityTypeBuilder<Hotel> builder)
    {
        base.Configure(builder);
        
        builder.Property(h => h.Id)
               .IsRequired();
        
        builder.Property(h => h.LegalName)
               .IsRequired()
               .HasMaxLength(250);
        
        builder.Property(h => h.CommercialName)
               .IsRequired()
               .HasMaxLength(250);
        
        builder.Property(h => h.Rnc)
               .IsRequired()
               .HasConversion(v => v.Value, v => new Rnc(v))
               .HasMaxLength(11);
        
        builder.Property(h => h.LogoUrl)
               .HasConversion(
                      v => v != null ? v.Value : null,
                      v => v != null ? new Url(v) : null)
               .HasMaxLength(2048);
        
        builder.Property(h => h.BusinessEmail)
               .IsRequired()
               .HasConversion(v => v.Value, v => new Email(v))
               .HasMaxLength(254);
        
        builder.Property(h => h.BusinessPhoneNumber)
               .IsRequired()
               .HasConversion(v => v.Value, v => new PhoneNumber(v))
               .HasMaxLength(25);

        builder.OwnsOne(h => h.Address, address =>
        {
               address.Property(a => a.Address1)
                      .IsRequired()
                      .HasMaxLength(200);

               address.Property(a => a.Address2)
                      .HasMaxLength(200)
                      .HasDefaultValue(null);

               address.Property(a => a.City)
                      .IsRequired()
                      .HasMaxLength(100);

               address.Property(a => a.Province)
                      .IsRequired()
                      .HasMaxLength(100);

               address.Property(a => a.ZipCode)
                      .IsRequired()
                      .HasMaxLength(11);
        });

        builder.OwnsOne(h => h.CheckOutPolicy, checkOutPolicy =>
        {
               checkOutPolicy.Property(c => c.CheckInTime)
                             .IsRequired();
               
               checkOutPolicy.Property(c => c.CheckOutTime)
                             .IsRequired();
        });

        builder.Property(h => h.DefaultCurrencyCode)
               .IsRequired()
               .HasConversion<string>();

        builder.Property(h => h.ItbisRate)
               .IsRequired()
               .HasConversion(v => v.Rate, v => new ItbisRate(v))
               .HasColumnType("decimal(10,2)");
        
        builder.Property(h => h.ServiceChargeRate)
               .IsRequired()
               .HasConversion(v => v.Rate, v => new ServiceChargeRate(v))
               .HasColumnType("decimal(10,2)");

        builder.Property(h => h.IsActive)
               .IsRequired()
               .HasDefaultValue(true);
    }
} 