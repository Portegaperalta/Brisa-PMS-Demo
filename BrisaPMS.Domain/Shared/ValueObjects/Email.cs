using System;
using BrisaPMS.Domain.Shared.Exceptions;

namespace BrisaPMS.Domain.Shared.ValueObjects
{
    public record Email
    {
        public string Value { get; }
        private const int MaxCharacterLimit = 254;

        public Email(string email)
        {
            if (string.IsNullOrWhiteSpace(email) is true)
                throw new EmptyRequiredFieldException("Email");

            if (email.Contains('@') is not true || email.Contains('.') is not true)
                throw new InvalidFieldException("Email", "The email must be a valid email address");

            if (email.Length > MaxCharacterLimit)
                throw new MaxCharacterLimitException(MaxCharacterLimit, "Email");

            Value = email;
        }
    }
}
