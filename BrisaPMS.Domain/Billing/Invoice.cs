using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Domain.Billing;

public class Invoice
{
    public Guid Id { get; init; }
    public Guid HotelId { get; init; }
    public Guid StayId { get; init; }
    public Guid IssuedBy { get; init; }
    public Ncf Ncf { get; init; }
    public InvoiceType Type { get; init; }
    public Rnc? GuestRnc { get; init; }
    public CurrencyCode CurrencyCode { get; init; }
    public Money ExchangeRate { get; init; }
    public Money RoomCharges { get; init; }
    public Money ServicesTotal { get; init; }
    public Money SubTotal { get; init; }
    public Money DiscountTotal { get; init; }
    public Money NetTotal { get; init; }
    public Money ItbisAmount { get; init; }
    public Money LegalTipTotal { get; init; }
    public Money PaidAmount { get; init; }
    public InvoiceStatus Status { get; init; }
    public DateTime IssuedAt { get; init; }

    public Invoice
    (
        Guid hotelId,
        Guid stayId,
        Guid issuedBy,
        Ncf ncf,
        InvoiceType type,
        CurrencyCode currencyCode,
        Money exchangeRate,
        Money roomCharges,
        Money servicesTotal,
        Money subTotal,
        Money discountTotal,
        Money netTotal,
        Money itbisAmount,
        Money legalTipTotal,
        Money paidAmount,
        InvoiceStatus status,
        Rnc? guestRnc = null
    )
    {
        if (hotelId == Guid.Empty)
            throw new EmptyRequiredFieldException("Hotel Id");

        if (stayId == Guid.Empty)
            throw new EmptyRequiredFieldException("Stay Id");

        if (issuedBy == Guid.Empty)
            throw new EmptyRequiredFieldException("Issued By");

        if (Enum.IsDefined<InvoiceType>(type) is not true)
            throw new BusinessRuleException("Invoice type not supported");

        if (Enum.IsDefined<CurrencyCode>(currencyCode) is not true)
            throw new BusinessRuleException("Currency code not supported");

        if (paidAmount.Amount < netTotal.Amount)
            throw new BusinessRuleException("Paid amount can't be less than net total amount");

        Id = Guid.CreateVersion7();
        HotelId = hotelId;
        StayId = stayId;
        IssuedBy = issuedBy;
        Ncf = ncf;
        Type = type;
        GuestRnc = guestRnc;
        CurrencyCode = currencyCode;
        ExchangeRate = exchangeRate;
        RoomCharges = roomCharges;
        ServicesTotal = servicesTotal;
        SubTotal = subTotal;
        DiscountTotal = discountTotal;
        NetTotal = netTotal;
        ItbisAmount = itbisAmount;
        LegalTipTotal = legalTipTotal;
        PaidAmount = paidAmount;
        Status = status;
        IssuedAt = DateTime.UtcNow;
    }
}