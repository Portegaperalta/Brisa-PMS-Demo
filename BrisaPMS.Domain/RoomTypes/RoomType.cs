using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Shared.Exceptions;

namespace BrisaPMS.Domain.RoomTypes;

public class RoomType
{
    public Guid Id { get; init; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public RoomBaseRate BaseRate {get; private set;}
    public int TotalBeds { get; private set; }
    public BedType BedType { get; private set; }
    public OccupancyPolicy OccupancyPolicy { get; private set; }

    public RoomType
    (
        string name,
        RoomBaseRate baseRate,
        int totalBeds,
        BedType  bedType,
        OccupancyPolicy occupancyPolicy,
        string? description = null
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new EmptyRequiredFieldException("Room type name");
        
        if (totalBeds is < 1 or > 20)
            throw new BusinessRuleException("Amount of Beds must be between 1 and 20");
        
        if (Enum.IsDefined<BedType>(bedType) is not true)
            throw new BusinessRuleException("Bed type not supported");

        Id = Guid.CreateVersion7();
        Name = name;
        Description = description;
        BaseRate = baseRate;
        TotalBeds = totalBeds;
        BedType = bedType;
        OccupancyPolicy = occupancyPolicy;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new EmptyRequiredFieldException("Room type name");
        
        Name = newName;
    }
    
    public void UpdateDescription(string newDescription)
        => Description = newDescription;

    public void UpdateBaseRate(RoomBaseRate newBaseRate) => BaseRate = newBaseRate;

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

    public void UpdateOccupancyPolicy(OccupancyPolicy newOccupancyPolicy) 
        => OccupancyPolicy = newOccupancyPolicy;
}