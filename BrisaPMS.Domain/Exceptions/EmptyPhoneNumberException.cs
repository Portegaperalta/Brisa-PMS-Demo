using System;

namespace BrisaPMS.Domain.Exceptions;

public class EmptyPhoneNumberException : Exception
{
    public EmptyPhoneNumberException() : base("Phone number cannot be empty") { }
}