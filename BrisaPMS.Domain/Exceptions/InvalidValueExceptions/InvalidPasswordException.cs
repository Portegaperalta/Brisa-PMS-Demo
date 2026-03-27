using System;

namespace BrisaPMS.Domain.Exceptions.InvalidValueExceptions
{
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException() : base("Password must be at least 8 characters and include an uppercase letter and a special character.") { }
    }
}
