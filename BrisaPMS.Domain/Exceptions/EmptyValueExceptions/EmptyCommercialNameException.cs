using System;

namespace BrisaPMS.Domain.Exceptions.EmptyValueExceptions;

public class EmptyCommercialNameException : Exception
{
    public EmptyCommercialNameException() : base("Commercial name cannot be empty") {}
}