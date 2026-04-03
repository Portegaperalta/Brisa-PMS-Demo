using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.Exceptions;

namespace BrisaPMS.Domain.Shared.ValueObjects;

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