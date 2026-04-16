using BrisaPMS.Domain.Guest;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Domain.Guests
{
    public class Guest
    {
        public Guid Id { get; init; }
        public Guid HotelId { get; init; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public GuestDocumentType DocumentType { get; private set; }
        public string DocumentNumber { get; private set; }
        public string? Country { get; private set; }
        public Rnc? Rnc { get; private set; }
        public Email Email { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public CurrencyCode PreferredCurrency { get; private set; }
        public string? PreferredLanguage { get; private set; }
        public bool IsVip { get; private set; }
        public bool IsBlackListed { get; private set; } = false;
        public string? BlackListedReason { get; private set; }
        public string? Notes { get; private set; }

        private Guest() {}
        
        // Nested Builder
        public class Builder
        {
            private readonly Guid _hotelId;
            private readonly string _firstName;
            private readonly string _lastName;
            private readonly GuestDocumentType _documentType;
            private readonly string _documentNumber;
            private readonly Email _email;
            private readonly PhoneNumber _phoneNumber;
            private readonly CurrencyCode _preferredCurrency;
            private readonly bool _isVip;
            
            private string? _country = null;
            private Rnc? _rnc = null;
            private string? _preferredLanguage = null;
            private string? _notes = null;

            public Builder
            (
                Guid hotelId,
                string firstName,
                string lastName,
                GuestDocumentType documentType,
                string documentNumber,
                Email email,
                PhoneNumber phoneNumber,
                CurrencyCode preferredCurrency,
                bool isVip
            )
            {
                if (hotelId == Guid.Empty)
                    throw new EmptyRequiredFieldException("Hotel Id");
                
                if (string.IsNullOrWhiteSpace(firstName))
                    throw new EmptyRequiredFieldException("First Name");
                
                if (string.IsNullOrWhiteSpace(lastName))
                    throw new EmptyRequiredFieldException("Last Name");
                
                if (string.IsNullOrWhiteSpace(documentNumber))
                    throw new EmptyRequiredFieldException("Document Number");
                
                _hotelId = hotelId;
                _firstName = firstName;
                _lastName = lastName;
                _documentType = documentType;
                _documentNumber = documentNumber;
                _email = email;
                _phoneNumber = phoneNumber;
                _preferredCurrency = preferredCurrency;
                _isVip = isVip;
            }

            public Builder WithCountry(string country) { _country = country; return this; }
            public Builder WithRnc(Rnc rnc) { _rnc = rnc; return this; }
            public Builder WithPreferredLanguage(string language) { _preferredLanguage = language; return this; }
            public Builder WithNotes(string notes) { _notes = notes; return this; }

            public Guest Build()
            {
                return new Guest()
                {
                    Id = Guid.CreateVersion7(),
                    HotelId = _hotelId,
                    FirstName = _firstName,
                    LastName = _lastName,
                    DocumentType = _documentType,
                    DocumentNumber = _documentNumber,
                    Country = _country,
                    Rnc = _rnc,
                    Email = _email,
                    PhoneNumber = _phoneNumber,
                    PreferredCurrency = _preferredCurrency,
                    PreferredLanguage = _preferredLanguage,
                    IsVip = _isVip,
                    Notes = _notes
                };
            }
        }
        
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

        public void ChangeDocumentType(GuestDocumentType newDocumentType)
        {
            if (Enum.IsDefined<GuestDocumentType>(newDocumentType) is not true)
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