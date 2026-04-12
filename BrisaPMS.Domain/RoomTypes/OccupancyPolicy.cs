using BrisaPMS.Domain.Shared.Exceptions;

namespace BrisaPMS.Domain.RoomTypes;

public record OccupancyPolicy
{
    public int MaxOccupancyAdults { get; }
    public int MaxOccupancyChildren { get; }

    public OccupancyPolicy(int maxOccupancyAdults, int maxOccupancyChildren)
    {
        if (maxOccupancyAdults is <= 0 or > 16)
            throw new BusinessRuleException("Max occupancy adults must be between 1 and 16");
        
        if (maxOccupancyChildren is < 0 or > 10)
            throw new BusinessRuleException("Max occupancy children must be  between 0 and 10");
        
        MaxOccupancyAdults = maxOccupancyAdults;
        MaxOccupancyChildren = maxOccupancyChildren;
    }
}