namespace BrisaPMS.Domain.Exceptions;

public class EmptyProvinceFieldException : Exception
{
    public EmptyProvinceFieldException() : base("Province field cannot be empty") {}
}