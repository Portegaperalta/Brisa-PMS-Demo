using BrisaPMS.Domain.Enums;
using BrisaPMS.Domain.Exceptions;
using BrisaPMS.Domain.ValueObjects;
using System;

namespace BrisaPMS.Domain.Entities
{
    public class Guest
    {
        //Attributes 
        public Guid Id { get; }
        public Guid HotelId { get; init; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DocumentType DocumentType { get; private set; }
        public string DocumentNumber { get; private set; }
        public string? Country { get; private set; }
        public Rnc? Rnc { get; private set; }
        public Email? Email { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public CurrencyCode PreferredCurrency { get; private set; }
        public string? PreferredLanguage { get; private set; }
        public bool IsVip { get; private set; }
        public bool IsBlackListed { get; private set; }
        public string? BlackListedReason { get; private set; }
        public string? Notes { get; private set; }

        // Constructor
        public Guest(Guid hotelId,
            string firstName,
            string lastName,
            DocumentType documentType,
            string documentNumber,
            PhoneNumber phoneNumber,
            CurrencyCode preferredCurrency,
            bool isVip,
            string? country = null,
            Rnc? rnc = null,
            Email? email = null,
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
            HotelId = hotelId;
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
        }
        
        // Behavioral methods
        public void ChangeFirstName(string newFirstName)
        {
            if (string.IsNullOrWhiteSpace(newFirstName))
                throw new EmptyRequiredFieldException("First Name");
            
            FirstName = newFirstName;
        }

        public void ChangeLastName(string newLastName)
        {
            if (string.IsNullOrWhiteSpace(newLastName))
                throw new EmptyRequiredFieldException("Last Name");
            
            LastName = newLastName;
        }

        public void ChangeDocumentType(DocumentType newDocumentType)
        {
            if (Enum.IsDefined<DocumentType>(newDocumentType) is not true)
                throw new BusinessRuleException("Document type not supported");
            
            DocumentType = newDocumentType;
        }

        public void ChangeDocumentNumber(string newDocumentNumber)
        {
            if (string.IsNullOrWhiteSpace(newDocumentNumber))
                throw new EmptyRequiredFieldException("Document Number");
            
            DocumentNumber = newDocumentNumber;
        }

        public void ChangeCountry(string newCountry)
        {
            if (string.IsNullOrWhiteSpace(newCountry))
                throw new EmptyRequiredFieldException("Country");
            
            Country = newCountry;
        }
        
        public void ChangeRnc(Rnc newRnc) =>  Rnc = newRnc;
        
        public void ChangeEmail(Email newEmail) =>  Email = newEmail;

        public void ChangePhoneNumber(PhoneNumber newPhoneNumber) => PhoneNumber = newPhoneNumber;

        public void ChangePreferredCurrency(CurrencyCode newPreferredCurrency)
        {
            if (Enum.IsDefined<CurrencyCode>(newPreferredCurrency) is not true)
                throw new BusinessRuleException("Currency not supported");
            
            PreferredCurrency = newPreferredCurrency;
        }

        public void ChangePreferredLanguage(string newPreferredLanguage)
        {
            if (string.IsNullOrWhiteSpace(newPreferredLanguage))
                throw new EmptyRequiredFieldException("Preferred Language");
            
            PreferredLanguage = newPreferredLanguage;
        }
        
        public void EnableVip () => IsVip = true;
        
        public void DisableVip() => IsVip = false;

        public void BlackList(string blackListedReason)
        {
            if (string.IsNullOrWhiteSpace(blackListedReason))
                throw new BusinessRuleException("Must have a reason to blacklist guest");
            
            BlackListedReason = blackListedReason;
            IsBlackListed = true;
        }
        
        public void DisableBlackList () => IsBlackListed = false;

        public void ChangeBlackListedReason(string newBlackListedReason) => BlackListedReason = newBlackListedReason;

        public void EditNotes(string newNotes)  => Notes = newNotes;
    }
}