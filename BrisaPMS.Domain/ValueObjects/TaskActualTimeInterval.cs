using BrisaPMS.Domain.Exceptions;

namespace BrisaPMS.Domain.ValueObjects;

public class TaskActualTimeInterval
{
    public DateTime ActualStartAt { get; }
    public DateTime ActualEndAt { get; }

    public TaskActualTimeInterval(DateTime actualStartAt, DateTime actualEndAt)
    {
        if (actualStartAt > actualEndAt)
            throw new BusinessRuleException("Actual start time must be earlier than actual end time.");
        
        if (actualEndAt < actualStartAt)
            throw new BusinessRuleException("Actual end time must be later than actual start time.");
        
        ActualStartAt = actualStartAt;
        ActualEndAt = actualEndAt;
    }
}