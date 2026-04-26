using BrisaPMS.Domain.HouseKeeping;
using BrisaPMS.Domain.Rooms;
using BrisaPMS.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrisaPMS.Persistence.Configurations;

public class HouseKeepingTaskConfig : BaseEntityConfig<HouseKeepingTask>
{
    public override void Configure(EntityTypeBuilder<HouseKeepingTask> builder)
    {
        base.Configure(builder);

        builder.Property(t => t.Id)
               .IsRequired();

        builder.HasOne<Room>()
               .WithMany()
               .HasForeignKey(r => r.RoomId)
               .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(t => t.AssignedBy)
               .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(t => t.AssignedTo)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(t => t.Type)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(20);

        builder.Property(t => t.Priority)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(20);

        builder.Property(t => t.Status)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(20);

        builder.Property(t => t.Notes)
               .HasMaxLength(500);

        builder.OwnsOne(t => t.Deadline, deadline =>
        {
               deadline.Property(d => d.ExpectedStartAt)
                       .IsRequired();
               
               deadline.Property(d => d.ExpectedEndAt)
                       .IsRequired();
        });

        builder.OwnsOne(t => t.ActualTimeInterval, actualTimeInterval =>
        {
               actualTimeInterval.Property(a => a.ActualStartAt);

               actualTimeInterval.Property(a => a.ActualEndAt);
        });

        builder.Property(t => t.IncidentReported)
               .IsRequired();

        builder.Property(t => t.IncidentDescription)
               .HasMaxLength(500);
    }
}