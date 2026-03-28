using BrisaPMS.Domain.Exceptions;

namespace BrisaPMS.Domain.ValueObjects;

public class CheckInOutTimes
{
    public DateTime CheckInTime { get; }
    public DateTime CheckOutTime { get; }

    public CheckInOutTimes(DateTime checkInTime, DateTime checkOutTime)
    {
        if (checkInTime >  checkOutTime)
            throw new InvalidFieldException("CheckIn Time", "cannot be greater than CheckOut TIme");
        
        if (checkOutTime < checkInTime)
            throw new InvalidFieldException("CheckOut Time", "cannot be less than CheckIn Time");
        
        CheckInTime = checkInTime;
        CheckOutTime = checkOutTime;
    }
}