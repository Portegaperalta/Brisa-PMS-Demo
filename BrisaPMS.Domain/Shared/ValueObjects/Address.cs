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

        private const int MaxAddress1CharacterLimit = 200;
        private const int MaxAddress2CharacterLimit = 200;
        private const int MaxCityCharacterLimit = 100;
        private const int MaxProvinceCharacterLimit = 100;
        private const int MaxZipcodeCharacterLimit = 11;

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

            if (address1.Length > MaxAddress1CharacterLimit)
                throw new MaxCharacterLimitException(MaxAddress1CharacterLimit, "Address 1");

            if (address2 is not null)
            {
                if (address2!.Length > MaxAddress2CharacterLimit)
                    throw new MaxCharacterLimitException(MaxAddress2CharacterLimit, "Address 2");
            }

            if (city.Length > MaxCityCharacterLimit)
                throw new MaxCharacterLimitException(MaxCityCharacterLimit, "City");

            if (province.Length > MaxProvinceCharacterLimit)
                throw new MaxCharacterLimitException(MaxProvinceCharacterLimit, "Province");

            if (zipcode.Length > MaxZipcodeCharacterLimit)
                throw new MaxCharacterLimitException(MaxZipcodeCharacterLimit, "Zip Code");

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
