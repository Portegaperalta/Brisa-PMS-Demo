using BrisaPMS.Domain.Companies;
using BrisaPMS.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrisaPMS.Persistence.Configurations;

public class CompanyConfig : BaseEntityConfig<Company>
{
    public override void Configure(EntityTypeBuilder<Company> builder)
    {
        base.Configure(builder);

        builder.Property(c => c.Id)
               .IsRequired();

        builder.Property(c => c.LegalName)
               .IsRequired()
               .HasMaxLength(250);
        
        builder.Property(c => c.CommercialName)
               .IsRequired()
               .HasMaxLength(250);
        
        builder.Property(c => c.Rnc)
               .IsRequired()
               .HasConversion(v => v.Value, v => new Rnc(v))
               .HasMaxLength(11);
        
        builder.Property(c => c.BusinessEmail)
               .IsRequired()
               .HasConversion(v => v.Value, v => new Email(v))
               .HasMaxLength(254);
        
        builder.Property(c => c.BusinessPhone)
               .IsRequired()
               .HasConversion(v => v.Value, v => new PhoneNumber(v))
               .HasMaxLength(25);
        
        builder.Property(c => c.LogoUrl)
               .HasConversion(
                      v => v != null ? v.Value : null,
                      v => v != null ? new Url(v) : null)
               .HasMaxLength(2048);
        
        builder.OwnsOne(c => c.Address, address =>
        {
               address.Property(a => a.Address1)
                      .IsRequired()
                      .HasMaxLength(200);

               address.Property(a => a.Address2)
                      .HasMaxLength(200);

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
    }
}