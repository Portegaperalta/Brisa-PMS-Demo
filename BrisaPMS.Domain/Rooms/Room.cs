using BrisaPMS.Domain.Shared.Exceptions;

namespace BrisaPMS.Domain.Rooms;

public class Room
{
    public Guid Id { get; init; }
    public Guid HotelId { get; init ; }
    public Guid RoomTypeId { get; private set; }
    public string Number { get; private set ; }
    public int Floor { get; init ; }
    public RoomAvailabilityStatus AvailabilityStatus { get; private set ; }
    public RoomHygieneStatus HygieneStatus { get; private set ; }
    public DateTime? LastCleanedAt { get; private set ; }
    public Guid? LastCleanedBy { get; private set ; }
    public bool NeedsRestocking { get; private set ; }
    
    public Room
    (
        Guid hotelId,
        Guid roomTypeId,
        string number,
        int floor,
        RoomAvailabilityStatus availabilityStatus,
        RoomHygieneStatus hygieneStatus
    )
    {
        if (hotelId == Guid.Empty)
            throw new EmptyRequiredFieldException("HotelId");
        
        if (roomTypeId == Guid.Empty)
            throw new EmptyRequiredFieldException("RoomTypeId");
        
        if(string.IsNullOrWhiteSpace(number))
            throw new EmptyRequiredFieldException("Room number");
        
        if(Enum.IsDefined<RoomAvailabilityStatus>(availabilityStatus) is false)
            throw new BusinessRuleException("Invalid availability status");
        
        if (Enum.IsDefined<RoomHygieneStatus>(hygieneStatus) is false)
            throw new BusinessRuleException("Invalid hygiene status");

        Id = Guid.CreateVersion7();
        HotelId = hotelId;
        RoomTypeId = roomTypeId;
        Number = number;
        Floor = floor;
        AvailabilityStatus = availabilityStatus;
        HygieneStatus = hygieneStatus;
        LastCleanedAt = null;
        LastCleanedBy = null;
        NeedsRestocking = false;
    }

    public void UpdateRoomType(Guid  newRoomTypeId)
    {
        if (newRoomTypeId == Guid.Empty)
            throw new EmptyRequiredFieldException("Room TypeId");
        
        RoomTypeId = newRoomTypeId;
    }

    public void UpdateNumber(string newRoomNumber)
    {
        if (string.IsNullOrWhiteSpace(newRoomNumber))
            throw new EmptyRequiredFieldException("Room number");

        Number = newRoomNumber;
    }

    public void UpdateAvailabilityStatus(RoomAvailabilityStatus newAvailabilityStatus)
    {
        if(Enum.IsDefined<RoomAvailabilityStatus>(newAvailabilityStatus) is false)
            throw new BusinessRuleException("Invalid availability status");
        
        AvailabilityStatus = newAvailabilityStatus;
    }

    public void UpdateHygieneStatus(RoomHygieneStatus newHygieneStatus)
    {
        if (Enum.IsDefined<RoomHygieneStatus>(newHygieneStatus) is false)
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