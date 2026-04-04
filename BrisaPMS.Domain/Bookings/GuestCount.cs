using BrisaPMS.Domain.Shared.Exceptions;

namespace BrisaPMS.Domain.Bookings;

public record GuestCount
{
    public int NumberOfAdults { get; }
    public int NumberOfChildren { get; }

    public GuestCount(int numberOfAdults, int numberOfChildren)
    {
        NumberOfAdults = numberOfAdults switch
        {
            <= 0 => throw new BusinessRuleException("Booking must include at least one adult"),
            > 10 => throw new BusinessRuleException("Booking can't exceed 10 adults"),
            _ => numberOfAdults
        };

        NumberOfChildren = numberOfChildren switch
        {
            < 0 => throw new BusinessRuleException("Number of children can't be negative"),
            > 10 => throw new BusinessRuleException("Booking can't exceed 10 children"),
            _ => numberOfChildren
        };
    }
}