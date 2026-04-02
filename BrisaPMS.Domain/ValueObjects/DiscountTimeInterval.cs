using BrisaPMS.Domain.Exceptions;

namespace BrisaPMS.Domain.ValueObjects;

public record DiscountTimeInterval
{
    public DateTime ValidFrom { get; }
    public DateTime ValidTo { get; }

    public DiscountTimeInterval(DateTime validFrom, DateTime validTo)
    {
        if (validFrom > validTo)
            throw new BusinessRuleException("Discount start date can't be older than its expiration date");
        
        ValidFrom = validFrom;
        ValidTo = validTo;
    }
}