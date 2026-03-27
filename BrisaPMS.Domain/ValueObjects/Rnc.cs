using System;
using System.Text.RegularExpressions;
using BrisaPMS.Domain.Exceptions.EmptyValueExceptions;
using BrisaPMS.Domain.Exceptions.InvalidValueExceptions;

namespace BrisaPMS.Domain.ValueObjects
{
    public record Rnc
    {
        public string Value { get; }

        private static readonly Regex DigitsOnly = new(@"\D", RegexOptions.Compiled);
        private readonly int BusinessRncLength = 9;
        private readonly int PersonRncLength = 11;

        public Rnc(string rawRnc)
        {
            if (string.IsNullOrWhiteSpace(rawRnc) is true)
                throw new EmptyRncException();

            var digits = DigitsOnly.Replace(rawRnc.Trim(), "");

            if (digits.All(char.IsDigit) is false)
                throw new InvalidRncException();

            if (digits.Length != PersonRncLength && digits.Length != BusinessRncLength)
                throw new InvalidRncException();

            Value = digits;
        }
    }
}
