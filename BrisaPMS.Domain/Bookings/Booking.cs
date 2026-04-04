using BrisaPMS.Domain.Booking;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Domain.Bookings;

public class Booking
{
    public Guid Id { get; init; }
    public Guid HotelId { get; init; }
    public Guid RoomId { get; init; }
    public Guid GuestId { get; init; }
    public string Source { get; private set; }
    public GuestCount GuestCount { get; private set; }
    public CheckInOutTimes CheckInOutTimes { get; private set; }
    public string? SpecialRequests { get; private set; }
    public BookingStatus Status { get; private set; }
    public string? CancellationReason { get;  private set; }
    public Money TotalPrice { get; private set; }
    public Guid? DiscountId { get; private set; }

    public Booking
    (
        Guid hotelId,
        Guid roomId,
        Guid guestId,
        string source,
        GuestCount guestCount,
        CheckInOutTimes checkInOutTimes,
        Money totalPrice,
        string? specialRequests = null,
        Guid? discountId = null
    ) 
    {
        if (hotelId ==  Guid.Empty)
            throw new EmptyRequiredFieldException("HotelId can't be empty");
        
        if (roomId ==  Guid.Empty)
            throw new EmptyRequiredFieldException("RoomId can't be empty");
        
        if (guestId ==  Guid.Empty)
            throw new EmptyRequiredFieldException("GuestId can't be empty");
        
        if (string.IsNullOrWhiteSpace(source))
            throw new EmptyRequiredFieldException("Booking source can't be empty");

        Id = Guid.CreateVersion7();
        HotelId = hotelId;
        RoomId = roomId;
        GuestId = guestId;
        Source = source;
        GuestCount =  guestCount;
        CheckInOutTimes = checkInOutTimes;
        SpecialRequests = specialRequests;
        Status = BookingStatus.Pending;
        CancellationReason = null;
        TotalPrice = totalPrice;
        DiscountId = discountId;
    }

    public void UpdateSource(string newSource)
    {
        if (string.IsNullOrWhiteSpace(newSource))
            throw new EmptyRequiredFieldException("Booking Source");
        
        Source = newSource;
    }
    
    public void UpdateGuestCount(GuestCount newGuestCount)
        => GuestCount = newGuestCount;

    public void UpdateCheckInOutTimes(CheckInOutTimes newCheckInOutTimes) 
        => CheckInOutTimes = newCheckInOutTimes;
    
    public void UpdateSpecialRequests(string newSpecialRequests) => SpecialRequests = newSpecialRequests;
    
    public void UpdateCancellationReason(string newCancellationReason)
    {
        if (string.IsNullOrWhiteSpace(newCancellationReason))
            throw new EmptyRequiredFieldException("Cancellation reason can't be empty");
        
        CancellationReason = newCancellationReason;
    }

    public void UpdateTotalPrice(Money newTotalPrice) => TotalPrice = newTotalPrice;

    public void UpdateDiscountId(Guid newDiscountId) => DiscountId = newDiscountId;
}