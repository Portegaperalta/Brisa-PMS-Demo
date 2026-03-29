namespace BrisaPMS.Domain.Exceptions;

public class CurrencyNotSupportedException : Exception
{
    public CurrencyNotSupportedException() : base("Currency not supported") {}
}