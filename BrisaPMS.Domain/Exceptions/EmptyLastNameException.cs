namespace BrisaPMS.Domain.Exceptions;

public class EmptyLastNameException : Exception
{
    public EmptyLastNameException() : base("Last name cannot be empty") { }
}