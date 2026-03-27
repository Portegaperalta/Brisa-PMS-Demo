using System;

namespace BrisaPMS.Domain.Exceptions.EmptyValueExceptions;

public class EmptyEmailException : Exception
{
    public EmptyEmailException() : base("Email cannot be empty") { }
}