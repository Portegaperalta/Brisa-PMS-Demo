namespace BrisaPMS.Domain.Billing;

public enum InvoiceStatus
{
    Sent = 1,
    PartiallyPaid = 2,
    Paid = 3,
    Cancelled = 4,
    Refunded = 5,
}