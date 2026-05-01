using BrisaPMS.Domain.Shared.ValueObjects;
using BrisaPMS.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrisaPMS.Persistence.Configurations;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Id)
               .IsRequired();
        
        builder.Property(u => u.Role)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(25);

        builder.Property(u => u.HotelId);

        builder.Property(u => u.FirstName)
               .IsRequired()
               .HasMaxLength(250);
        
        builder.Property(u => u.LastName)
               .IsRequired()
               .HasMaxLength(250);
        
        builder.Property(u => u.Email)
               .IsRequired()
               .HasConversion(v => v.Value, v => new Email(v))
               .HasMaxLength(254);

        builder.Property(u => u.PhoneNumber)
               .HasConversion(
                      v => v != null ? v.Value : null,
                      v => v != null ? new PhoneNumber(v) : null
               )
               .HasMaxLength(25);
        
        builder.Property(u => u.PreferredLanguage)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(5);

        builder.Property(u => u.IsActive)
               .IsRequired();
    }
}