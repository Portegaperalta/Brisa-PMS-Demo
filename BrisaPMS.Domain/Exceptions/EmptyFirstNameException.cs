namespace BrisaPMS.Domain.Exceptions;

public class EmptyFirstNameException : Exception
{
    public EmptyFirstNameException() : base("First name cannot be empty") { }
}