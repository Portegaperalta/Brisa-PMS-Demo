namespace BrisaPMS.Domain.Exceptions;

public class EmptyZipCodeException : Exception
{
    public EmptyZipCodeException() : base("Zip code cannot be empty.") {}
}