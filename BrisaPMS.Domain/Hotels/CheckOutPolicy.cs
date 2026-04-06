using BrisaPMS.Domain.Shared.Exceptions;

namespace BrisaPMS.Domain.Hotels;

public class CheckOutPolicy
{
    public TimeOnly CheckInTime { get; }
    public TimeOnly CheckOutTime { get; }

    public CheckOutPolicy(TimeOnly checkInTime, TimeOnly checkOutTime)
    {
        if (checkInTime > checkOutTime)
            throw new BusinessRuleException("Check-In time can't be greater than Check-Out time.");
        
        if (checkOutTime < checkInTime)
            throw new BusinessRuleException("Check-Out time can't be less than Check-In time.");
        
        if (checkInTime ==  checkOutTime)
            throw new BusinessRuleException("Check-In time can't be the same as Check-Out time.");
        
        CheckInTime = checkInTime;
        CheckOutTime = checkOutTime;
    }
}