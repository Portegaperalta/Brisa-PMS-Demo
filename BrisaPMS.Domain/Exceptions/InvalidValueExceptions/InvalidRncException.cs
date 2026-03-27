using System;

namespace BrisaPMS.Domain.Exceptions.InvalidValueExceptions
{
    public class InvalidRncException : Exception
    {
        public InvalidRncException() : 
            base("Invalid RNC, must contain digits only and be between 9 (Business) and 11 (Person) digits.") { }
    }
}
