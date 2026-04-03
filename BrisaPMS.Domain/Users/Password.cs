using BrisaPMS.Domain.Shared.Exceptions;
using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace BrisaPMS.Domain.Users
{
    public record Password
    {
        public string Value { get; }

        private const int MaxCharacterLimit = 512;
        private const int MinCharacterLimit = 8;
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100_000;
        private readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;
        private readonly Regex SpecialCharRegex = new(@"[!@#$%^&*()\-_+=\[\]{}|\\;:'"",.<>/?`~]", RegexOptions.Compiled);

        public Password(string password)
        {
            if (string.IsNullOrWhiteSpace(password) is true)
                throw new EmptyRequiredFieldException("Password");

            if (password.Length > MaxCharacterLimit)
                throw new InvalidFieldException("Password", "cannot be more than 512 characters long");

            if (password.Length < MinCharacterLimit)
                throw new InvalidFieldException("Password", "must be at least 8 characters long");

            if (SpecialCharRegex.IsMatch(password) is false)
                throw new InvalidFieldException("Password", "must include at least one special character.");

            if (password.Any(char.IsUpper) is false)
                throw new InvalidFieldException("Password", "must include an uppercase letter.");

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
