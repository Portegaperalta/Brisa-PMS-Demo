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
    public BookingSource Source { get; private set; }
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
        BookingSource source,
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

        if (Enum.IsDefined<BookingSource>(source) is not true)
            throw new BusinessRuleException("Booking source not supported");

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

    public void UpdateSource(BookingSource newSource)
    {
        if (Enum.IsDefined<BookingSource>(newSource) is not true)
            throw new BusinessRuleException("Booking source not supported");

        Source = Status switch
        {
            BookingStatus.Complete => throw new BusinessRuleException(
                "Booking is already completed, unable to modify source"),
            
            BookingStatus.Cancelled => throw new BusinessRuleException(
                "Booking is already cancelled, unable to modify source"),
            
            _ => newSource
        };
    }

    public void UpdateGuestCount(GuestCount newGuestCount)
    {
        GuestCount = Status switch
        {
            BookingStatus.Complete => throw new BusinessRuleException(
                "Booking is already completed, unable to modify guest count"),
            
            BookingStatus.Cancelled => throw new BusinessRuleException(
                "Booking is already cancelled, unable to modify guest count"),
            
            _ => newGuestCount
        };
    }

    public void UpdateCheckInOutTimes(CheckInOutTimes newCheckInOutTimes)
    {
        CheckInOutTimes = Status switch
        {
            BookingStatus.Complete => throw new BusinessRuleException(
                "Booking is already completed, unable to modify CheckIn-Out Times"),
            
            BookingStatus.Cancelled => throw new BusinessRuleException(
                "Booking is already cancelled, unable to modify CheckIn-Out Times"),
            
            _ => newCheckInOutTimes
        };
    }

    public void UpdateSpecialRequests(string newSpecialRequests)
    {
        SpecialRequests = Status switch
        {
            BookingStatus.Complete => throw new BusinessRuleException(
                "Booking is already completed, unable to modify special requests"),
            
            BookingStatus.Cancelled => throw new BusinessRuleException(
                "Booking is already cancelled, unable to modify special requests"),
            
            _ => newSpecialRequests
        };
    }

    public void SetAsConfirmed()
    {
        Status = Status switch
        {
            BookingStatus.Cancelled => throw new BusinessRuleException("Cancelled booking can't be set as confirmed"),
            BookingStatus.Complete => throw new BusinessRuleException("Completed booking can't be set as confirmed"),
            BookingStatus.Confirmed => throw new BusinessRuleException("Booking is already confirmed"),
            _ => Status = BookingStatus.Confirmed
        };
    }
    
    public void SetAsCompleted()
    {
        Status = Status switch
        {
            BookingStatus.Cancelled => throw new BusinessRuleException("Cancelled booking can't be set as completed"),
            BookingStatus.Complete => throw new BusinessRuleException("Booking is already completed"),
            _ => Status = BookingStatus.Complete
        };
    }
    
    public void SetAsCancelled(string cancellationReason)
    {
        if (string.IsNullOrWhiteSpace(cancellationReason))
            throw new EmptyRequiredFieldException("Cancellation reason can't be empty");

        Status = Status switch
        {
            BookingStatus.Complete => throw new BusinessRuleException("Completed booking can't be cancelled"),
            BookingStatus.Cancelled => throw new BusinessRuleException("Booking is already cancelled"),
            _ => Status = BookingStatus.Cancelled
        };
        
        CancellationReason = cancellationReason;
    }
    
    public void UpdateCancellationReason(string newCancellationReason)
    {
        if (string.IsNullOrWhiteSpace(newCancellationReason))
            throw new EmptyRequiredFieldException("Cancellation reason can't be empty");
        
        if (Status is not BookingStatus.Cancelled)
            throw new BusinessRuleException("Booking must be cancelled to be able to modify cancellation reason");
        
        CancellationReason = newCancellationReason;
    }

    public void UpdateTotalPrice(Money newTotalPrice)
    {
        TotalPrice = Status switch
        {
            BookingStatus.Complete => throw new BusinessRuleException(
                "Booking is already completed, unable to modify total price"),

            BookingStatus.Cancelled => throw new BusinessRuleException(
                "Booking is already cancelled, unable to modify total price"),

            _ => newTotalPrice
        };
    }

    public void UpdateDiscountId(Guid newDiscountId)
    {
        DiscountId = Status switch
        {
            BookingStatus.Complete => throw new BusinessRuleException(
                "Booking is already completed, unable to change discount"),
            
            BookingStatus.Cancelled => throw new BusinessRuleException(
                "Booking is already cancelled, unable to change discount"),
            
            _ => newDiscountId
        };
    }
}