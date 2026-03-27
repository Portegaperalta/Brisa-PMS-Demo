using System;

namespace BrisaPMS.Domain.Exceptions.EmptyValueExceptions;

public class EmptyProvinceFieldException : Exception
{
    public EmptyProvinceFieldException() : base("Province field cannot be empty") {}
}