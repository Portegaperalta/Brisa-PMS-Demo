using System;

namespace BrisaPMS.Domain.Exceptions.EmptyValueExceptions;

public class EmptyLegalNameException : Exception
{
    public EmptyLegalNameException() : base("Lega name cannot be empty") {}
}