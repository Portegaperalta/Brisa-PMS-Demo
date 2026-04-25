using BrisaPMS.Domain.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrisaPMS.Persistence.Configurations;

public abstract class BaseEntityConfig<T> : IEntityTypeConfiguration<T>
    where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(p => p.CreatedBy)
               .IsRequired();

        builder.Property(p => p.CreatedAt)
               .IsRequired();

        builder.Property(p => p.UpdatedBy)
               .HasDefaultValue(null);

        builder.Property(p => p.UpdatedAt)
            .HasDefaultValue(null);
    }
}