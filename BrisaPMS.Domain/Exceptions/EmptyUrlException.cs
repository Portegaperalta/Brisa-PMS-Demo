using System;

namespace BrisaPMS.Domain.Exceptions
{
    public class EmptyUrlException : Exception
    {
        public EmptyUrlException() : base("Url cannot be empty") { }
    }
}
