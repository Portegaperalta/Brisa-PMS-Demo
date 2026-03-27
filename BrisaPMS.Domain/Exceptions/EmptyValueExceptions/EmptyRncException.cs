using System;

namespace BrisaPMS.Domain.Exceptions.EmptyValueExceptions;

public class EmptyRncException : Exception
{
    public EmptyRncException() : base("Rnc cannot be empty.") {} 
}