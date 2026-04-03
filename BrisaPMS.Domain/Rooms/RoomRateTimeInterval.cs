using BrisaPMS.Domain.Shared.Exceptions;

namespace BrisaPMS.Domain.Rooms;

public class RoomRateTimeInterval
{
    public DateTime ValidFrom { get; }
    public DateTime ValidTo { get; }

    public RoomRateTimeInterval(DateTime validFrom, DateTime validTo)
    {
        if (validFrom > validTo)
            throw new BusinessRuleException("Rate start time must be less than or equal to end time");
        
        if (validTo < validFrom)
            throw new BusinessRuleException("Rate end time must be greater than start time");
        
        ValidFrom = validFrom;
        ValidTo = validTo;
    }
}