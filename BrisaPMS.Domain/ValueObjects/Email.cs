using BrisaPMS.Domain.Exceptions;
using BrisaPMS.Domain.Exceptions.EmptyValueExceptions;
using BrisaPMS.Domain.Exceptions.InvalidValueExceptions;

namespace BrisaPMS.Domain.ValueObjects
{
    public record Email
    {
        public string Value { get; }
        private readonly int MaxCharacterLimit = 254;

        public Email(string email)
        {
            if (string.IsNullOrWhiteSpace(email) is true)
                throw new EmptyEmailException();

            if (email.Contains('@') != true && email.Contains('.') != true)
                throw new InvalidEmailAddressException();

            if (email.Length > 254)
                throw new MaxCharacterLimitException(MaxCharacterLimit, "Email");

            Value = email;
        }
    }
}
