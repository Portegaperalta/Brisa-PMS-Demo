namespace BrisaPMS.Domain.Exceptions;

public class EmptyCommercialNameException : Exception
{
    public EmptyCommercialNameException() : base("Commercial name cannot be empty") {}
}