using BrisaPMS.Domain.Amenities;
using BrisaPMS.Domain.RoomTypes;
using BrisaPMS.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrisaPMS.Persistence.Configurations
{
    public class AmenityRoomTypeConfig : IEntityTypeConfiguration<AmenityRoomType>
    {
        public void Configure(EntityTypeBuilder<AmenityRoomType> builder)
        {
            builder.ToTable("AmenityRoomType");

            builder.HasKey(ar => new { ar.AmenityId, ar.RoomTypeId });

            builder.HasOne<Amenity>()
                   .WithMany()
                   .HasForeignKey(ar => ar.AmenityId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<RoomType>()
                   .WithMany()
                   .HasForeignKey(ar => ar.RoomTypeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
