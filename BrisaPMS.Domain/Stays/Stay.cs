using BrisaPMS.Domain.Shared.Abstractions;
using BrisaPMS.Domain.Shared.Exceptions;

namespace BrisaPMS.Domain.Stays;

public class Stay : BaseEntity
{
    public Guid Id { get; init; }
    public Guid HotelId { get; init; }
    public Guid RoomId { get; init; }
    public Guid GuestId { get; init; }
    public Guid BookingId {get; init;}
    public StayTimeInterval TimeInterval {get; private set;}
    public int NightCount { get; private set; }
    public StayStatus Status { get; private set; }

    public Stay(Guid hotelId, Guid roomId, Guid guestId, Guid bookingId)
    {
        if (hotelId == Guid.Empty)
            throw new EmptyRequiredFieldException("HotelId");
        
        if (roomId == Guid.Empty)
            throw new EmptyRequiredFieldException("RoomId");
        
        if (guestId == Guid.Empty)
            throw new EmptyRequiredFieldException("GuestId");
        
        if (bookingId == Guid.Empty)
            throw new EmptyRequiredFieldException("BookingId");

        Id = Guid.CreateVersion7();
        HotelId = hotelId;
        RoomId = roomId;
        GuestId = guestId;
        BookingId = bookingId;
        TimeInterval = new StayTimeInterval(DateTime.UtcNow);
        NightCount = 0;
        Status = StayStatus.InProgress;
    }
    
    public void IncreaseNightCount()
    {
        if (Status == StayStatus.Complete)
            throw new BusinessRuleException("Night count can't be increased once stay it's completed");
        
        NightCount++;
    }

    public void SetAsComplete()
    {
        Status = StayStatus.Complete;
        TimeInterval = new StayTimeInterval(TimeInterval.ActualCheckIn, DateTime.UtcNow);
    }
}