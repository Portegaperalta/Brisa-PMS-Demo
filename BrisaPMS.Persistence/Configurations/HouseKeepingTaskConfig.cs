using BrisaPMS.Domain.HouseKeeping;
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
        
        builder.Property(t => t.RoomId)
               .IsRequired();
        
        builder.Property(t => t.AssignedBy)
               .IsRequired();
        
        builder.Property(t => t.AssignedTo)
               .IsRequired();

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
               .HasMaxLength(20)
               .HasDefaultValue(HouseKeepingTaskStatus.Pending);

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
               actualTimeInterval.Property(a => a.ActualStartAt)
                                 .HasDefaultValue(null);
               
               actualTimeInterval.Property(a => a.ActualEndAt)
                                 .HasDefaultValue(null);
        });
        
        builder.Property(t => t.IncidentReported)
               .IsRequired()
               .HasDefaultValue(false);

        builder.Property(t => t.IncidentDescription)
               .HasMaxLength(500);
    }
}