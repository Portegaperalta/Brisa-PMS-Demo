using BrisaPMS.Domain.Shared.Exceptions;
using System;

namespace BrisaPMS.Domain.Shared.ValueObjects
{
    public record PhoneNumber
    {
        public string Value { get; }

        private const int MaxCharacterLimit = 25;

        public PhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber) is true)
                throw new EmptyRequiredFieldException("Phone Number");

            if (phoneNumber.Length > MaxCharacterLimit)
                throw new MaxCharacterLimitException(MaxCharacterLimit, "Phone number");

            Value = phoneNumber;
        }
    }
}
