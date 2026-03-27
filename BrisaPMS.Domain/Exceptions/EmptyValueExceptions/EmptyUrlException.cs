using System;

namespace BrisaPMS.Domain.Exceptions.EmptyValueExceptions
{
    public class EmptyUrlException : Exception
    {
        public EmptyUrlException() : base("Url cannot be empty") { }
    }
}
