using System;

namespace BrisaPMS.Domain.Exceptions;

public class EmptyPreferredLanguageException : Exception
{
    public EmptyPreferredLanguageException() : base("Preferred language cannot be empty") { }   
}