using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.RoomTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrisaPMS.Persistence.Configurations;

public class RoomTypeConfig : BaseEntityConfig<RoomType>
{
    public override void Configure(EntityTypeBuilder<RoomType> builder)
    {
        base.Configure(builder);

        builder.Property(rt => rt.Id)
               .IsRequired();
        
        builder.Property(rt => rt.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(rt => rt.Description)
            .HasMaxLength(500);

        builder.Property(rt => rt.BaseRate)
               .IsRequired()
               .HasConversion(
                   v => v.Rate,
                   v => new RoomBaseRate(v)
                );

        builder.OwnsOne(rt => rt.Beds, beds =>
        {
            beds.Property(b => b.BedType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            beds.Property(b => b.NumberOfBeds)
                .IsRequired()
                .HasMaxLength(20);
        });

        builder.OwnsOne(rt => rt.OccupancyPolicy, occupancyPolicy =>
        {
            occupancyPolicy.Property(op => op.MaxOccupancyAdults)
                           .IsRequired()
                           .HasMaxLength(16);

            occupancyPolicy.Property(op => op.MaxOccupancyChildren)
                           .IsRequired()
                           .HasMaxLength(10);
        });
    }
}