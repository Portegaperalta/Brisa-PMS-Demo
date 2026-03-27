using System;

namespace BrisaPMS.Domain.Exceptions.EmptyValueExceptions;

public class EmptyFirstNameException : Exception
{
    public EmptyFirstNameException() : base("First name cannot be empty") { }
}