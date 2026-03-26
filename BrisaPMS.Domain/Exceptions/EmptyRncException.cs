namespace BrisaPMS.Domain.Exceptions;

public class EmptyRncException : Exception
{
    public EmptyRncException() : base("Rnc cannot be empty.") {} 
}