using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Domain.Billing;

public class Payment
{
    public Guid Id { get; init; }
    public Guid HotelId { get; init; }
    public Guid GuestId { get; init; }
    public Guid BookingId { get; init; }
    public Guid InvoiceId { get; init; }
    public Money Amount { get; init; }
    public PaymentMethod PaymentMethod { get; init; }
    public string? ReferenceNumber { get; init; }
    public string? Notes { get;  private set; }

    public Payment
    (
        Guid hotelId,
        Guid guestId,
        Guid bookingId,
        Guid invoiceId,
        Money amount,
        PaymentMethod paymentMethod,
        string? referenceNumber = null,
        string? notes = null
    )
    {
        if (hotelId == Guid.Empty)
            throw new BusinessRuleException("Hotel Id can't be empty");
        
        if (guestId == Guid.Empty)
            throw new BusinessRuleException("Guest Id can't be empty");
        
        if (bookingId == Guid.Empty)
            throw new BusinessRuleException("Booking Id can't be empty");
        
        if (invoiceId == Guid.Empty)
            throw new BusinessRuleException("Invoice Id can't be empty");

        if (Enum.IsDefined<PaymentMethod>(paymentMethod) is not true)
            throw new BusinessRuleException("Payment method not supported");

        Id = Guid.CreateVersion7();
        HotelId = hotelId;
        GuestId = guestId;
        BookingId = bookingId;
        InvoiceId = invoiceId;
        Amount = amount;
        PaymentMethod = paymentMethod;
        ReferenceNumber = referenceNumber;
        Notes = notes;
    }

    public void UpdateNotes(string newNotes) => Notes = newNotes;
}