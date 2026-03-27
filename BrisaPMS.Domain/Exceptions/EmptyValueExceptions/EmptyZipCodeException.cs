using System;

namespace BrisaPMS.Domain.Exceptions.EmptyValueExceptions;

public class EmptyZipCodeException : Exception
{
    public EmptyZipCodeException() : base("Zip code cannot be empty.") {}
}