using BrisaPMS.Domain.Enums;
using BrisaPMS.Domain.Exceptions;

namespace BrisaPMS.Domain.Entities;

public class Room
{
    public Guid Id { get; init; }
    public Guid HotelId { get; init ; }
    public Guid RoomTypeId { get; private set; }
    public string Number { get; private set ; }
    public int Floor { get; init ; }
    public int TotalBeds { get; private set ; }
    public AvailabilityStatus AvailabilityStatus { get; private set ; }
    public HygieneStatus HygieneStatus { get; private set ; }
    public DateTime? LastCleanedAt { get; private set ; }
    public Guid? LastCleanedBy { get; private set ; }
    public bool NeedsRestocking { get; private set ; }
    
    public Room(Guid hotelId,
        Guid roomTypeId,
        string number,
        int floor,
        int totalBeds,
        AvailabilityStatus availabilityStatus,
        HygieneStatus hygieneStatus)
    {
        if (hotelId == Guid.Empty)
            throw new EmptyRequiredFieldException("HotelId");
        
        if (roomTypeId == Guid.Empty)
            throw new EmptyRequiredFieldException("RoomTypeId");
        
        if(string.IsNullOrWhiteSpace(number))
            throw new EmptyRequiredFieldException("Room number");
        
        if (totalBeds <= 0) 
            throw new BusinessRuleException("Room must have at least one bed");
        
        if(Enum.IsDefined<AvailabilityStatus>(availabilityStatus) is false)
            throw new BusinessRuleException("Invalid availability status");
        
        if (Enum.IsDefined<HygieneStatus>(hygieneStatus) is false)
            throw new BusinessRuleException("Invalid hygiene status");

        Id = Guid.CreateVersion7();
        HotelId = hotelId;
        RoomTypeId = roomTypeId;
        Number = number;
        Floor = floor;
        TotalBeds = totalBeds;
        AvailabilityStatus = availabilityStatus;
        HygieneStatus = hygieneStatus;
        LastCleanedAt = null;
        LastCleanedBy = null;
        NeedsRestocking = false;
    }

    public void ChangeRoomType(Guid  newRoomTypeId)
    {
        if (newRoomTypeId == Guid.Empty)
            throw new EmptyRequiredFieldException("Room TypeId");
        
        RoomTypeId = newRoomTypeId;
    }

    public void ChangeNumber(string newRoomNumber)
    {
        if (string.IsNullOrWhiteSpace(newRoomNumber))
            throw new EmptyRequiredFieldException("Room number");

        Number = newRoomNumber;
    }

    public void ChangeTotalBeds(int newTotalBeds)
    {
        if (newTotalBeds <= 0)
            throw new BusinessRuleException("Room must have at least one bed");
        
        TotalBeds = newTotalBeds;
    }

    public void ChangeAvailabilityStatus(AvailabilityStatus newAvailabilityStatus)
    {
        if(Enum.IsDefined<AvailabilityStatus>(newAvailabilityStatus) is false)
            throw new BusinessRuleException("Invalid availability status");
        
        AvailabilityStatus = newAvailabilityStatus;
    }

    public void ChangeHygieneStatus(HygieneStatus newHygieneStatus)
    {
        if (Enum.IsDefined<HygieneStatus>(newHygieneStatus) is false)
            throw new BusinessRuleException("Invalid hygiene status");
        
        HygieneStatus = newHygieneStatus;
    }
    
    public void UpdateLastCleanedAt() => LastCleanedAt = DateTime.UtcNow;

    public void UpdateLastCleanedBy(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new EmptyRequiredFieldException("User id");
        
        LastCleanedBy = userId;
    }

    public void SetAsPendingRestocking() => NeedsRestocking = true;
    
    public void SetAsRestocked() => NeedsRestocking = false;
}