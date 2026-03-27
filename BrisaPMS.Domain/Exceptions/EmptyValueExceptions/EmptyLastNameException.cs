using System;

namespace BrisaPMS.Domain.Exceptions.EmptyValueExceptions;

public class EmptyLastNameException : Exception
{
    public EmptyLastNameException() : base("Last name cannot be empty") { }
}