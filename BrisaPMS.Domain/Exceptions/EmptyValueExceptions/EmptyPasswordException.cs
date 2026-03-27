using System;

namespace BrisaPMS.Domain.Exceptions.EmptyValueExceptions;

public class EmptyPasswordException : Exception
{
    public EmptyPasswordException() : base("Password cannot be empty") { }
}