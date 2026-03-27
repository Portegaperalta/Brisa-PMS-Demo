using System;
using BrisaPMS.Domain.Exceptions;

namespace BrisaPMS.Domain.ValueObjects
{
    public record Email
    {
        public string Value { get; }
        private readonly int MaxCharacterLimit = 254;

        public Email(string email)
        {
            if (string.IsNullOrWhiteSpace(email) is true)
                throw new EmptyRequiredFieldException("Email");

            if (email.Contains('@') != true || email.Contains('.') != true)
                throw new InvalidFieldException("Email", "The email must be a valid email address");

            if (email.Length > MaxCharacterLimit)
                throw new MaxCharacterLimitException(MaxCharacterLimit, "Email");

            Value = email;
        }
    }
}
