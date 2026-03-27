using System;

namespace BrisaPMS.Domain.Exceptions
{
    public class InvalidUrlException : Exception
    {
       public InvalidUrlException() : base("URL must use http or https and have a valid URL format") { }
    }
}
