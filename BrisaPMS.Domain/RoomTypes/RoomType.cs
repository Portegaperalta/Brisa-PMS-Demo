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
        
        if (baseRate is < 0 or > 100)
            throw new BusinessRuleException("Base Rate must be between 0% and 100%");
        
        if (totalBeds is < 1 or > 20)
            throw new BusinessRuleException("Amount of Beds must be between 1 and 20");
        
        if (Enum.IsDefined<BedType>(bedType) is not true)
            throw new BusinessRuleException("Bed type not supported");
        
        if (maxOccupancyAdults is <= 0 or > 16)
            throw new BusinessRuleException("Max occupancy adults must be between 1 and 16");
        
        if (maxOccupancyChildren is < 0 or > 10)
            throw new BusinessRuleException("Max occupancy children must be  between 0 and 10");

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
        if (newBaseRate is < 0 or > 100)
            throw new BusinessRuleException("Base Rate must be between 0% and 100%");
        
        BaseRate = newBaseRate;
    }

    public void UpdateTotalBeds(int newTotalBeds)
    {
        if (newTotalBeds is < 1 or > 20)
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
        if (newMaxOccupancyAdults is < 1 or > 16)
            throw new BusinessRuleException("Max occupancy adults must be between 1 and 16");
        
        MaxOccupancyAdults = newMaxOccupancyAdults;
    }

    public void UpdateMaxOccupancyChildren(int newMaxOccupancyChildren)
    {
        if (newMaxOccupancyChildren is < 0 or > 10)
            throw new BusinessRuleException("Max occupancy children must be  between 0 and 10");
        
        MaxOccupancyChildren = newMaxOccupancyChildren;
    }
}