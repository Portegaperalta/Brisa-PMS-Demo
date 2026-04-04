namespace BrisaPMS.Domain.Billing;

public class InvalidServiceChargeRateException : Exception
{
    public InvalidServiceChargeRateException(string message) :  base(message) {}
}