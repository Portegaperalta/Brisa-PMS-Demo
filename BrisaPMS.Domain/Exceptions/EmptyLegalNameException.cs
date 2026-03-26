namespace BrisaPMS.Domain.Exceptions;

public class EmptyLegalNameException : Exception
{
    public EmptyLegalNameException() : base("Lega name cannot be empty") {}
}