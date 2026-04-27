using BrisaPMS.Domain.Shared.Exceptions;

namespace BrisaPMS.Domain.Hotels;

public record CheckOutPolicy
{
    public TimeOnly CheckInTime { get; }
    public TimeOnly CheckOutTime { get; }

    private CheckOutPolicy() { }

    public CheckOutPolicy(TimeOnly checkInTime, TimeOnly checkOutTime)
    {
        CheckInTime = checkInTime;
        CheckOutTime = checkOutTime;
    }
}