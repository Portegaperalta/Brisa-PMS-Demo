using BrisaPMS.Domain.Enums;
using BrisaPMS.Domain.Exceptions;

namespace BrisaPMS.Domain.ValueObjects;

public record Money
{
    public decimal Amount { get; }
    public CurrencyCode CurrencyCode { get; }

    public Money(decimal amount, CurrencyCode currencyCode = CurrencyCode.DOP)
    {
        if (amount < 0m)
            throw new BusinessRuleException("Amount cannot be negative");
        
        if (Enum.IsDefined<CurrencyCode>(currencyCode) is not true)
            throw new BusinessRuleException("Currency code not supported");
        
        Amount = amount;
        CurrencyCode = currencyCode;
    }
}