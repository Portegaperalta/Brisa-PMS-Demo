namespace BrisaPMS.Domain.Shared.Exceptions;

public class CurrencyNotSupportedException : Exception
{
    public CurrencyNotSupportedException() : base("Currency not supported") {}
}