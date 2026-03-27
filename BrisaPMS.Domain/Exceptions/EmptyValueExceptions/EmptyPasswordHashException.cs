using System;

namespace BrisaPMS.Domain.Exceptions.EmptyValueExceptions;

public class EmptyPasswordHashException : Exception
{
    public EmptyPasswordHashException() : base("Password hash cannot be empty") { }
}