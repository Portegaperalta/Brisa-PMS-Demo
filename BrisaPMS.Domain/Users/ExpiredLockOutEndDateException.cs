using System;

namespace BrisaPMS.Domain.Users;

public class ExpiredLockOutEndDateException : Exception
{
    public ExpiredLockOutEndDateException() : base("Lockout expiry date cannot be older than current date") { }
}
