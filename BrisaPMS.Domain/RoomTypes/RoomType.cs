using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Shared.Abstractions;
using BrisaPMS.Domain.Shared.Exceptions;

namespace BrisaPMS.Domain.RoomTypes;

public class RoomType : BaseEntity
{
    public Guid Id { get; init; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public RoomBaseRate BaseRate {get; private set;}
    public RoomBed Beds {get; private set;}
    public OccupancyPolicy OccupancyPolicy { get; private set; }

    public RoomType
    (
        string name,
        RoomBaseRate baseRate,
        RoomBed  beds,
        OccupancyPolicy occupancyPolicy,
        string? description = null
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new EmptyRequiredFieldException("Room type name");

        Id = Guid.CreateVersion7();
        Name = name;
        Description = description;
        BaseRate = baseRate;
        Beds =  beds;
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

    public void UpdateRoomBeds(RoomBed newRoomBeds) => Beds = newRoomBeds;

    public void UpdateOccupancyPolicy(OccupancyPolicy newOccupancyPolicy) 
        => OccupancyPolicy = newOccupancyPolicy;
}