using BrisaPMS.Domain.Enums;
using BrisaPMS.Domain.Exceptions;
using BrisaPMS.Domain.ValueObjects;
using System;

namespace BrisaPMS.Domain.Entities
{
    public class Guest
    {
        //Attributes 
        public Guid Id { get; private init; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DocumentType DocumentType { get; private set; }
        public string DocumentNumber { get; private set; }
        public string? Country { get; private set; }
        public Rnc? Rnc { get; private set; }
        public Email? Email { get; private set; }
        public PhoneNumber? PhoneNumber { get; private set; }
        public CurrencyCode PreferredCurrency { get; private set; }
        public string? PreferredLanguage { get; private set; }
        public bool IsVip { get; private set; }
        public bool IsBlackListed { get; private set; }
        public string? BlackListedReason { get; private set; }
        public string? Notes { get; private set; }
        public DateTime CreatedAt { get; private init; }
        public DateTime? UpdatedAt { get; private init; }

        // Constructor
        public Guest(string firstName,
            string lastName,
            DocumentType documentType,
            string documentNumber,
            CurrencyCode preferredCurrency,
            bool isVip,
            string? country = null,
            Rnc? rnc = null,
            Email? email = null,
            PhoneNumber? phoneNumber = null,
            string? preferredLanguage = null,
            string? notes = null)
        {
            if (string.IsNullOrWhiteSpace(firstName) is true)
                throw new EmptyRequiredFieldException("First Name");

            if (string.IsNullOrWhiteSpace(lastName) is true)
                throw new EmptyRequiredFieldException("Last Name");

            if (Enum.IsDefined<DocumentType>(documentType) is false)
                throw new BusinessRuleException("Document type not supported");

            if (string.IsNullOrWhiteSpace(documentNumber) is true)
                throw new BusinessRuleException("Document number cannot be empty");

            if (Enum.IsDefined<CurrencyCode>(preferredCurrency) is false)
                throw new BusinessRuleException("Currency not supported");

            Id = Guid.CreateVersion7();
            FirstName = firstName;
            LastName = lastName;
            DocumentType = documentType;
            DocumentNumber = documentNumber;
            Country = country;
            Rnc = rnc;
            Email = email;
            PhoneNumber = phoneNumber;
            PreferredCurrency = preferredCurrency;
            PreferredLanguage = preferredLanguage;
            IsVip = isVip;
            IsBlackListed = false;
            BlackListedReason = null;
            Notes = notes;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = null;
        }
    }
}