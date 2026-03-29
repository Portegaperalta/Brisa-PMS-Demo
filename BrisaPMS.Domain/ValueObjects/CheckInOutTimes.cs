using BrisaPMS.Domain.Exceptions;

namespace BrisaPMS.Domain.ValueObjects;

public class CheckInOutTimes
{
    public DateTime CheckInTime { get; }
    public DateTime CheckOutTime { get; }

    public CheckInOutTimes(DateTime checkInTime, DateTime checkOutTime)
    {
        if (checkInTime >  checkOutTime)
            throw new BusinessRuleException("Check-In Time cannot be greater than check-out time");
        
        if (checkOutTime < checkInTime)
            throw new BusinessRuleException("Check-Out time cannot be less than Check-In time");
        
        CheckInTime = checkInTime;
        CheckOutTime = checkOutTime;
    }
}