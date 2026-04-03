using System;

namespace BrisaPMS.Domain.Shared.Exceptions
{
    public class InvalidFieldException : Exception
    {
        public InvalidFieldException(string fieldName, string errorMessage) : base($"Invalid {fieldName}, {errorMessage}") { }
    }
}
