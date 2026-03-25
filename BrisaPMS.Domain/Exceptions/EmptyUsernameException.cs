namespace BrisaPMS.Domain.Exceptions;

public class EmptyUsernameException : Exception
{
    public EmptyUsernameException() : base("Username cannot be empty") { }
}