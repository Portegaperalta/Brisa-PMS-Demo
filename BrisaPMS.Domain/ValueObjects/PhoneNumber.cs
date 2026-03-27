using BrisaPMS.Domain.Exceptions;
using BrisaPMS.Domain.Exceptions.EmptyValueExceptions;
using System;

namespace BrisaPMS.Domain.ValueObjects
{
    public record PhoneNumber
    {
        public string Value { get; }

        private readonly int MaxCharacterLimit = 25;

        public PhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber) is true)
                throw new EmptyPhoneNumberException();

            if (phoneNumber.Length > MaxCharacterLimit)
                throw new MaxCharacterLimitException(MaxCharacterLimit, "Phone number");

            Value = phoneNumber;
        }
    }
}
