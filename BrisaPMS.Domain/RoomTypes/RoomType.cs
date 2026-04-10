using BrisaPMS.Domain.Shared.Exceptions;

namespace BrisaPMS.Domain.RoomTypes;

public class RoomType
{
    public Guid Id { get; init; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public decimal BaseRate {get; private set;}
    public int TotalBeds { get; private set; }
    public BedType BedType { get; private set; }
    public int MaxOccupancyAdults { get; private set ; }
    public int MaxOccupancyChildren { get; private set ; }

    public RoomType
    (
        string name,
        decimal baseRate,
        int totalBeds,
        BedType  bedType,
        int maxOccupancyAdults,
        int maxOccupancyChildren,
        string? description = null
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new EmptyRequiredFieldException("Room type name");
        
        if (baseRate < 0)
            throw new BusinessRuleException("BaseRate can't be negative");
        
        if (totalBeds <= 0)
            throw new BusinessRuleException("Room type must have at least 1 Bed");
        
        if (Enum.IsDefined<BedType>(bedType) is false)
            throw new BusinessRuleException("Bed type not supported");
        
        if (maxOccupancyAdults <= 0)
            throw new BusinessRuleException("Max Occupancy Adults can't be less  or equal than zero");
        
        if (maxOccupancyChildren < 0)
            throw new BusinessRuleException("Max Occupancy Children can't less than zero");

        Id = Guid.CreateVersion7();
        Name = name;
        Description = description;
        BaseRate = baseRate;
        TotalBeds = totalBeds;
        BedType = bedType;
        MaxOccupancyAdults = maxOccupancyAdults;
        MaxOccupancyChildren = maxOccupancyChildren;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new EmptyRequiredFieldException("Room type name");
        
        Name = newName;
    }
    
    public void UpdateDescription(string newDescription)
        => Description = newDescription;

    public void UpdateBaseRate(decimal newBaseRate)
    {
        if (newBaseRate < 0)
            throw new BusinessRuleException("BaseRate can't be negative");
        
        BaseRate = newBaseRate;
    }

    public void UpdateTotalBeds(int newTotalBeds)
    {
        if (newTotalBeds <= 0)
            throw new BusinessRuleException("Room type must have at least 1 Bed");
        
        TotalBeds = newTotalBeds;
    }

    public void UpdateBedType(BedType newBedType)
    {
        if (Enum.IsDefined<BedType>(newBedType) is false)
            throw new BusinessRuleException("Bed type not supported");
        
        BedType = newBedType;
    }

    public void UpdateMaxOccupancyAdults(int newMaxOccupancyAdults)
    {
        if (newMaxOccupancyAdults <= 0)
            throw new BusinessRuleException("Max Occupancy Adults can't be less or equal than zero");
        
        MaxOccupancyAdults = newMaxOccupancyAdults;
    }

    public void UpdateMaxOccupancyChildren(int newMaxOccupancyChildren)
    {
        if (newMaxOccupancyChildren < 0)
            throw new BusinessRuleException("Max Occupancy Children can't less than zero");
        
        MaxOccupancyChildren = newMaxOccupancyChildren;
    }
}