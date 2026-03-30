using System;
using BrisaPMS.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace BrisaPMS.Domain.ValueObjects
{
    public record Rnc
    {
        public string Value { get; }

        private static readonly Regex DigitsOnly = new(@"\D", RegexOptions.Compiled);
        private const int BusinessRncLength = 9;
        private const int PersonRncLength = 11;

        public Rnc(string rawRnc)
        {
            if (string.IsNullOrWhiteSpace(rawRnc) is true)
                throw new EmptyRequiredFieldException("Rnc");

            var digits = DigitsOnly.Replace(rawRnc.Trim(), "");

            if (digits.All(char.IsDigit) is false)
                throw new BusinessRuleException("Rnc must contain digits only.");

            if (digits.Length != PersonRncLength && digits.Length != BusinessRncLength)
                throw new BusinessRuleException("Rnc must be between 9 (Business) and 11 (Person) digits.");

            Value = digits;
        }
    }
}
