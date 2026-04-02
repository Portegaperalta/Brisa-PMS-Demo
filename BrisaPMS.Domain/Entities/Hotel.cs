using BrisaPMS.Domain.Enums;
using BrisaPMS.Domain.Exceptions;
using BrisaPMS.Domain.ValueObjects;

namespace BrisaPMS.Domain.Entities
{
    public class Hotel
    {
        public Guid Id { get; }
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
        
        // Constructor
        public Hotel
        (
            string legalName,
            string commercialName,
            Url logoUrl,
            Email businessEmail,
            PhoneNumber businessPhoneNumber,
            Address address,
            CheckInOutTimes checkInOutTimes,
            CurrencyCode defaultCurrencyCode,
            decimal itbisRate,
            decimal serviceChargeRate,
            bool isActive
        )
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
        }
        
        // Behavioral Methods
        public void UpdateLegalName(string newLegalName)
        {
            if (string.IsNullOrWhiteSpace(newLegalName) is true)
                throw new EmptyRequiredFieldException("Legal Name");
            
            LegalName = newLegalName;
        }

        public void UpdateCommercialName(string newCommercialName)
        {
            if (string.IsNullOrWhiteSpace(newCommercialName) is true)
                throw new EmptyRequiredFieldException("Commercial Name");
            
            CommercialName = newCommercialName;
        }
        
        public void UpdateLogoUrl(Url newLogoUrl) => LogoUrl = newLogoUrl;
        
        public void UpdateBusinessEmail(Email newBusinessEmail) => BusinessEmail = newBusinessEmail;
        
        public void UpdateBusinessPhoneNumber(PhoneNumber newBusinessPhoneNumber) 
            => BusinessPhoneNumber = newBusinessPhoneNumber;
        
        public void UpdateAddress(Address newAddress) => Address = newAddress;
        
        public void UpdateCheckInOutTimes (CheckInOutTimes newCheckInOutTimes) 
            => CheckInOutTimes = newCheckInOutTimes;

        public void UpdateDefaultCurrencyCode(CurrencyCode newDefaultCurrencyCode)
        {
            if (Enum.IsDefined<CurrencyCode>(newDefaultCurrencyCode) is not true)
                throw new CurrencyNotSupportedException();
            
            DefaultCurrencyCode = newDefaultCurrencyCode;
        }

        public void UpdateItbisRate(decimal newItbisRate)
        {
         if (newItbisRate < 0)
             throw new BusinessRuleException("Itbis Rate cannot be negative");
         
         ItbisRate = newItbisRate;
        }

        public void UpdateServiceChargeRate(decimal newServiceChargeRate)
        {
            if (newServiceChargeRate < 0)
                throw new BusinessRuleException("Service charge rate cannot be negative");
            
            ServiceChargeRate = newServiceChargeRate;
        }
    }
}
