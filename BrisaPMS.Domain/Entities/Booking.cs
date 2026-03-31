using BrisaPMS.Domain.Enums;
using BrisaPMS.Domain.Exceptions;
using BrisaPMS.Domain.ValueObjects;

namespace BrisaPMS.Domain.Entities;

public class Booking
{
    public Guid Id { get; init; }
    public Guid HotelId { get; init; }
    public Guid RoomId { get; init; }
    public Guid GuestId { get; init; }
    public string Source { get; private set; }
    public int NumberOfAdults { get; private set; }
    public int NumberOfChildren { get; private set; }
    public CheckInOutTimes CheckInOutTimes { get; private set; }
    public string? SpecialRequests { get; private set; }
    public BookingStatus Status { get; private set; }
    public string? CancellationReason { get;  private set; }
    public decimal TotalPrice { get; private set; }
    public Guid? DiscountId { get; private set; }

    public Booking
    (
        Guid hotelId,
        Guid roomId,
        Guid guestId,
        string source,
        int numberOfAdults,
        int numberOfChildren,
        CheckInOutTimes checkInOutTimes,
        decimal totalPrice,
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
        
        if (string.IsNullOrWhiteSpace(source) is true)
            throw new EmptyRequiredFieldException("Booking source can't be empty");
        
        if  (numberOfAdults <= 0)
            throw new BusinessRuleException("Booking must include at least one adult");
        
        if (numberOfChildren < 0)
            throw new BusinessRuleException("Number of children can't be less than zero");
        
        if (totalPrice < 0)
            throw new BusinessRuleException("Total price can't be less than zero");

        Id = Guid.CreateVersion7();
        HotelId = hotelId;
        RoomId = roomId;
        GuestId = guestId;
        Source = source;
        NumberOfAdults = numberOfAdults;
        NumberOfChildren = numberOfChildren;
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

    public void UpdateNumberOfAdults(int newNumberOfAdults)
    {
        if (newNumberOfAdults <= 0)
            throw new BusinessRuleException("Booking must include at least one adult");
            
        NumberOfAdults = newNumberOfAdults;
    }

    public void UpdateNumberOfChildren(int newNumberOfChildren)
    {
        if (newNumberOfChildren < 0)
            throw new BusinessRuleException("Number of children can't be less than zero");
        
        NumberOfChildren = newNumberOfChildren;
    }

    public void UpdateCheckInOutTimes(CheckInOutTimes newCheckInOutTimes) =>
        CheckInOutTimes = newCheckInOutTimes;
    
    public void UpdateSpecialRequests(string newSpecialRequests) => SpecialRequests = newSpecialRequests;
    
    public void UpdateCancellationReason(string newCancellationReason)
    {
        if (string.IsNullOrWhiteSpace(newCancellationReason))
            throw new EmptyRequiredFieldException("Cancellation reason can't be empty");
        
        CancellationReason = newCancellationReason;
    }

    public void UpdateTotalPrice(decimal newTotalPrice)
    {
        if (newTotalPrice < 0)
            throw new BusinessRuleException("Total price can't be less than zero");

        TotalPrice = newTotalPrice;
    }

    public void UpdateDiscountId(Guid newDiscountId) => DiscountId = newDiscountId;
}