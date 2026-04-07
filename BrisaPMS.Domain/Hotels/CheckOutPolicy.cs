using BrisaPMS.Domain.Shared.Exceptions;

namespace BrisaPMS.Domain.Hotels;

public record CheckOutPolicy
{
    public TimeOnly CheckInTime { get; }
    public TimeOnly CheckOutTime { get; }

    public CheckOutPolicy(TimeOnly checkInTime, TimeOnly checkOutTime)
    {
        CheckInTime = checkInTime;
        CheckOutTime = checkOutTime;
    }
}