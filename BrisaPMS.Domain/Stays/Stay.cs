using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Stay;

namespace BrisaPMS.Domain.Stays;

public class Stay
{
    public Guid Id { get; init; }
    public Guid GuestId { get; init; }
    public Guid BookingId {get; init;}
    public DateTime ActualCheckIn { get; private set; }
    public DateTime? ActualCheckOut { get; private set; }
    public int NightCount { get; private set; }
    public StayStatus Status { get; private set; }

    public Stay(Guid guestId, Guid bookingId)
    {
        if (guestId == Guid.Empty)
            throw new EmptyRequiredFieldException("GuestId");
        
        if (bookingId == Guid.Empty)
            throw new EmptyRequiredFieldException("BookingId");

        Id = Guid.CreateVersion7();
        GuestId = guestId;
        BookingId = bookingId;
        ActualCheckIn = DateTime.UtcNow;
        ActualCheckOut = null;
        NightCount = 0;
        Status = StayStatus.InProgress;
    }
    
    public void IncreaseNightCount()
    {
        if (Status == StayStatus.Complete || Status == StayStatus.Cancelled)
            throw new BusinessRuleException("Stay night count can't be increased once it's completed or cancelled");

        NightCount += 1;
    }

    public void SetAsComplete()
    {
        if (Status == StayStatus.Cancelled)
            throw new BusinessRuleException("Canceled stay cant be set as completed");

        Status = StayStatus.Complete;
        ActualCheckOut = DateTime.UtcNow;
    }

    public void SetAsCancelled()
    {
        if (Status == StayStatus.Complete)
            throw new BusinessRuleException("Completed stay cant be set as cancelled");
        
        Status = StayStatus.Cancelled;
    }
}