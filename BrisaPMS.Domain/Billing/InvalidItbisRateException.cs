namespace BrisaPMS.Domain.Billing;

public class InvalidItbisRateException : Exception
{
    public InvalidItbisRateException(string message) : base(message) {}
}