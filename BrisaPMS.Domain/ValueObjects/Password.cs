using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using BrisaPMS.Domain.Exceptions.EmptyValueExceptions;
using BrisaPMS.Domain.Exceptions.InvalidValueExceptions;

namespace BrisaPMS.Domain.ValueObjects
{
    public record Password
    {
        public string Value { get; }

        private readonly int MaxCharacterLimit = 512;
        private readonly int MinCharacterLimit = 8;
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100_000;
        private readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA3_256;
        private readonly Regex SpecialCharRegex = new(@"[!@#$%^&*()\-_+=\[\]{}|\\;:'"",.<>/?`~]", RegexOptions.Compiled);

        public Password(string password)
        {
            if (string.IsNullOrWhiteSpace(password) is true)
                throw new EmptyPasswordException();

            if (password.Length < MinCharacterLimit)
                throw new InvalidPasswordException();

            if (SpecialCharRegex.IsMatch(password) is false)
                throw new InvalidPasswordException();

            if (password.Any(char.IsUpper) is false)
                throw new InvalidPasswordException();

            Value = Hash(password);
        }

        private string Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }
    }
}
