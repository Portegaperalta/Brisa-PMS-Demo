namespace BrisaPMS.Domain.Exceptions;

class ExpiredLockoutEndDateException : Exception
{
    public ExpiredLockoutEndDateException() : base("Lockout expiry date cannot be older than current date") { }
}
