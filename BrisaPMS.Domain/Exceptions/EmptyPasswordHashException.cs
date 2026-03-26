using System;

namespace BrisaPMS.Domain.Exceptions;

public class EmptyPasswordHashException : Exception
{
    public EmptyPasswordHashException() : base("Password hash cannot be empty") { }
}