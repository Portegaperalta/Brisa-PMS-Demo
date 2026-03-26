namespace BrisaPMS.Domain.Exceptions;

public class ExpiredLockOutEndDateException : Exception
{
    public ExpiredLockOutEndDateException() : base("Lockout expiry date cannot be older than current date") { }
}
