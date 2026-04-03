using System;

namespace BrisaPMS.Domain.HouseKeeping;

public record TaskActualTimeInterval
{
    public DateTime ActualStartAt { get; }
    public DateTime ActualEndAt { get; }

    public TaskActualTimeInterval(DateTime actualStartAt, DateTime actualEndAt)
    {
        ActualStartAt = actualStartAt;
        ActualEndAt = actualEndAt;
    }
}