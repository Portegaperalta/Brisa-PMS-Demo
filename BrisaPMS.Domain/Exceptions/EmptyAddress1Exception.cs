namespace BrisaPMS.Domain.Exceptions;

public class EmptyAddress1Exception : Exception
{
    public EmptyAddress1Exception() : base("Address 1 cannot be empty") {}
}