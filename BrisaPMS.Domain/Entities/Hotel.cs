using BrisaPMS.Domain.Enums;
using BrisaPMS.Domain.Exceptions;
using BrisaPMS.Domain.ValueObjects;

namespace BrisaPMS.Domain.Entities
{
    public class Hotel
    {
        public Guid Id { get; private init; }
        public string LegalName { get; private set; }
        public string CommercialName { get; private set; }
        public Url LogoUrl { get; private set; }
        public Email BusinessEmail { get; private set; }
        public PhoneNumber BusinessPhoneNumber { get; private set; }
        public Address Address { get; private set; }
        public CheckInOutTimes CheckInOutTimes { get; private set; }
        public CurrencyCode DefaultCurrencyCode { get; private set; }
        public decimal ItbisRate { get; private set; }
        public decimal ServiceChargeRate  { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private init; }
        public DateTime? UpdatedAt { get; private set; }
        
        // Constructor
        public Hotel(string legalName,
            string commercialName,
            Url logoUrl,
            Email businessEmail,
            PhoneNumber businessPhoneNumber,
            Address address,
            CheckInOutTimes checkInOutTimes,
            CurrencyCode defaultCurrencyCode,
            decimal itbisRate,
            decimal serviceChargeRate,
            bool isActive)
        {
            if (string.IsNullOrWhiteSpace(legalName) is true)
                throw new EmptyRequiredFieldException("Legal Name");
            
            if (string.IsNullOrWhiteSpace(commercialName) is true)
                throw new EmptyRequiredFieldException("Commercial Name");
            
            if (Enum.IsDefined<CurrencyCode>(defaultCurrencyCode) is not true)
                throw new CurrencyNotSupportedException();
            
            if (itbisRate < 0)
                throw new BusinessRuleException("Itbis Rate cannot be negative");
            
            if (serviceChargeRate < 0)
                throw new BusinessRuleException("Service charge rate cannot be negative");
            
            Id = Guid.CreateVersion7();
            LegalName = legalName;
            CommercialName = commercialName;
            LogoUrl = logoUrl;
            BusinessEmail = businessEmail;
            BusinessPhoneNumber = businessPhoneNumber;
            Address = address;
            CheckInOutTimes = checkInOutTimes;
            DefaultCurrencyCode = defaultCurrencyCode;
            ItbisRate = itbisRate;
            ServiceChargeRate = serviceChargeRate;
            IsActive = isActive;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = null;
        }
        
        // Behavioral Methods
        public void ChangeLegalName(string newLegalName)
        {
            if (string.IsNullOrWhiteSpace(newLegalName) is true)
                throw new EmptyRequiredFieldException("Legal Name");
            
            LegalName = newLegalName;
        }

        public void ChangeCommercialName(string newCommercialName)
        {
            if (string.IsNullOrWhiteSpace(newCommercialName) is true)
                throw new EmptyRequiredFieldException("Commercial Name");
            
            CommercialName = newCommercialName;
        }
        
        public void ChangeLogoUrl(Url newLogoUrl) => LogoUrl = newLogoUrl;
        
        public void ChangeBusinessEmail(Email newBusinessEmail) => BusinessEmail = newBusinessEmail;
        
        public void ChangeBusinessPhoneNumber(PhoneNumber newBusinessPhoneNumber) 
            => BusinessPhoneNumber = newBusinessPhoneNumber;
        
        public void ChangeAddress(Address newAddress) => Address = newAddress;
        
        public void ChangeCheckInOutTimes (CheckInOutTimes newCheckInOutTimes) 
            => CheckInOutTimes = newCheckInOutTimes;

        public void ChangeDefaultCurrencyCode(CurrencyCode newDefaultCurrencyCode)
        {
            if (Enum.IsDefined<CurrencyCode>(newDefaultCurrencyCode) is not true)
                throw new CurrencyNotSupportedException();
            
            DefaultCurrencyCode = newDefaultCurrencyCode;
        }

        public void ChangeItbisRate(decimal newItbisRate)
        {
         if (newItbisRate < 0)
             throw new BusinessRuleException("Itbis Rate cannot be negative");
         
         ItbisRate = newItbisRate;
        }

        public void ChangeServiceChargeRate(decimal newServiceChargeRate)
        {
            if (newServiceChargeRate < 0)
                throw new BusinessRuleException("Service charge rate cannot be negative");
            
            ServiceChargeRate = newServiceChargeRate;
        }
        
        public void DisableIsActive() => IsActive = false;
        
        public void EnableIsActive() => IsActive = true;
        
        public void UpdateLastUpdatedAt() => UpdatedAt = DateTime.UtcNow;
    }
}
