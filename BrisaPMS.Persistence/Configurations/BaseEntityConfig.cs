using BrisaPMS.Domain.Shared.Abstractions;
using BrisaPMS.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrisaPMS.Persistence.Configurations;

public abstract class BaseEntityConfig<T> : IEntityTypeConfiguration<T>
    where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(x => x.CreatedBy)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(p => p.CreatedAt)
               .IsRequired();

        builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(x => x.UpdatedBy)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(p => p.UpdatedAt)
            .HasDefaultValue(null);
    }
}