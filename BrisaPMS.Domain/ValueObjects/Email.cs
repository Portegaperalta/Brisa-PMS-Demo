using BrisaPMS.Domain.Exceptions;

namespace BrisaPMS.Domain.ValueObjects
{
    public record Email
    {
        public string Value { get; }

        public Email(string email)
        {
            if (string.IsNullOrWhiteSpace(email) is true)
                throw new EmptyEmailException();

            if (email.Contains('@') != true && email.Contains('.') != true)
                throw new InvalidEmailAddressException();

            Value = email;
        }
    }
}
