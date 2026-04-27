using BrisaPMS.Domain.Shared.Exceptions;

namespace BrisaPMS.Domain.Shared.ValueObjects
{
    public record PhoneNumber
    {
        public string Value { get; }

        private PhoneNumber() { }

        public PhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber) is true)
                throw new EmptyRequiredFieldException("Phone Number");

            Value = phoneNumber;
        }
    }
}
