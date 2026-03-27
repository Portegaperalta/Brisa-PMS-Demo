using System;

namespace BrisaPMS.Domain.Exceptions.EmptyValueExceptions;

public class EmptyAddress1Exception : Exception
{
    public EmptyAddress1Exception() : base("Address 1 cannot be empty") {}
}