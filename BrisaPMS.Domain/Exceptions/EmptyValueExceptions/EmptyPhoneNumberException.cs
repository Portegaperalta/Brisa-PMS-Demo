using System;

namespace BrisaPMS.Domain.Exceptions.EmptyValueExceptions;

public class EmptyPhoneNumberException : Exception
{
    public EmptyPhoneNumberException() : base("Phone number cannot be empty") { }
}