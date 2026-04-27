using BrisaPMS.Domain.Shared.ValueObjects;
using BrisaPMS.Domain.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrisaPMS.Persistence.Configurations;

public class UserConfig : BaseEntityConfig<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

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

        builder.Property(u => u.PasswordHash)
               .IsRequired()
               .HasConversion(v => v.Value, v => new Password(v))
               .HasMaxLength(512);

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
        
        builder.Property(u => u.IsEmailConfirmed)
               .IsRequired();
        
        builder.Property(u => u.FailedLoginAttempts)
               .IsRequired();

        builder.Property(u => u.LockOutDuration);

        builder.Property(u => u.LockOutEnd);

        builder.Property(u => u.LastLoginAt);

        builder.Property(u => u.PasswordChangedAt);
    }
}