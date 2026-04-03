using System;

namespace BrisaPMS.Domain.User;

public class ExpiredLockOutEndDateException : Exception
{
    public ExpiredLockOutEndDateException() : base("Lockout expiry date cannot be older than current date") { }
}
