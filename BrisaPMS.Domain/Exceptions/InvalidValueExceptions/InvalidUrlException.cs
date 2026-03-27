using System;

namespace BrisaPMS.Domain.Exceptions.InvalidValueExceptions
{
    public class InvalidUrlException : Exception
    {
       public InvalidUrlException() : base("URL must use http or https and have a valid URL format") { }
    }
}
