using System;

namespace BrisaPMS.Domain.Exceptions.EmptyValueExceptions;

public class EmptyCityFieldException : Exception
{
    public EmptyCityFieldException() : base("City field cannot be empty") {}
}