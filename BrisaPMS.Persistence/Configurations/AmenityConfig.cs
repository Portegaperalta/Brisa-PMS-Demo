using BrisaPMS.Domain.Amenities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrisaPMS.Persistence.Configurations
{
    public class AmenityConfig : BaseEntityConfig<Amenity>
    {
        public override void Configure(EntityTypeBuilder<Amenity> builder)
        {
            base.Configure(builder);  
            
            builder.Property(p => p.Id)
                .IsRequired();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(p => p.IsActive)
                .IsRequired();
        }
    }
}
