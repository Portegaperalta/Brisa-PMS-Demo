using System;
using BrisaPMS.Domain.Shared.Exceptions;

namespace BrisaPMS.Domain.Shared.ValueObjects
{
    public record Address
    {
        public string Address1 { get; }
        public string? Address2 { get; }
        public string City { get; }
        public string Province { get; }
        public string ZipCode { get; }

        private Address() { }

        public Address(string address1, string? address2, string city, string province, string zipcode)
        {
            if (string.IsNullOrWhiteSpace(address1) is true)
                throw new EmptyRequiredFieldException("Address 1");

            if (string.IsNullOrWhiteSpace(city) is true)
                throw new EmptyRequiredFieldException("City");

            if (string.IsNullOrWhiteSpace(province) is true)
                throw new EmptyRequiredFieldException("Province");

            if (string.IsNullOrWhiteSpace(zipcode) is true)
                throw new EmptyRequiredFieldException("Zip Code");

            if (zipcode.All(char.IsDigit) is not true)
                throw new BusinessRuleException("Zip Code can only contain numeric values");

            Address1 = address1;
            Address2 = address2;
            City = city;
            Province = province;
            ZipCode = zipcode;
        }
    }
}
