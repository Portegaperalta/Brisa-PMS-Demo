using System;
using BrisaPMS.Domain.Shared.Exceptions;

namespace BrisaPMS.Domain.HouseKeeping;

public record TaskExpectedTimeInterval
{
    public DateTime ExpectedStartAt { get; }
    public DateTime ExpectedEndAt { get; }

    public TaskExpectedTimeInterval(DateTime expectedStartAt, DateTime expectedEndAt)
    {
        if (expectedStartAt > expectedEndAt)
            throw new BusinessRuleException("Expected start time must be earlier than Expected end time.");
        
        if (expectedEndAt < expectedStartAt)
            throw new BusinessRuleException("Expected end time must be later than Expected start time.");
        
        if (expectedStartAt == expectedEndAt)
            throw new BusinessRuleException("Expected start time and end time cannot be the same.");
        
        ExpectedStartAt = expectedStartAt;
        ExpectedEndAt = expectedEndAt;
    }
}