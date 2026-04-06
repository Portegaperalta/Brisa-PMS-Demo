using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Domain.Hotels
{
    public class Hotel
    {
        public Guid Id { get; init; }
        public string LegalName { get; private set; }
        public string CommercialName { get; private set; }
        public Url? LogoUrl { get; private set; }
        public Email BusinessEmail { get; private set; }
        public PhoneNumber BusinessPhoneNumber { get; private set; }
        public Address Address { get; private set; }
        public CheckOutPolicy CheckOutPolicy { get; private set; }
        public CurrencyCode DefaultCurrencyCode { get; private set; }
        public ItbisRate ItbisRate { get; private set; }
        public ServiceChargeRate ServiceChargeRate  { get; private set; }
        public bool IsActive { get;  private set; }
        
        // Constructor
        public Hotel
        (
            string legalName,
            string commercialName,
            Email businessEmail,
            PhoneNumber businessPhoneNumber,
            Address address,
            CheckOutPolicy checkOutPolicy,
            ItbisRate itbisRate,
            ServiceChargeRate serviceChargeRate,
            bool isActive = true,
            Url? logoUrl = null,
            CurrencyCode defaultCurrencyCode = CurrencyCode.DOP
        )
        {
            if (string.IsNullOrWhiteSpace(legalName))
                throw new EmptyRequiredFieldException("Legal Name");
            
            if (string.IsNullOrWhiteSpace(commercialName))
                throw new EmptyRequiredFieldException("Commercial Name");
            
            if (Enum.IsDefined<CurrencyCode>(defaultCurrencyCode) is not true)
                throw new CurrencyNotSupportedException();
            
            Id = Guid.CreateVersion7();
            LegalName = legalName;
            CommercialName = commercialName;
            LogoUrl = logoUrl;
            BusinessEmail = businessEmail;
            BusinessPhoneNumber = businessPhoneNumber;
            Address = address;
            CheckOutPolicy = checkOutPolicy;
            DefaultCurrencyCode = defaultCurrencyCode;
            ItbisRate = itbisRate;
            ServiceChargeRate = serviceChargeRate;
            IsActive = isActive;
        }
        
        // Behavioral Methods
        public void UpdateLegalName(string newLegalName)
        {
            if (string.IsNullOrWhiteSpace(newLegalName))
                throw new EmptyRequiredFieldException("Legal Name");
            
            LegalName = newLegalName;
        }

        public void UpdateCommercialName(string newCommercialName)
        {
            if (string.IsNullOrWhiteSpace(newCommercialName))
                throw new EmptyRequiredFieldException("Commercial Name");
            
            CommercialName = newCommercialName;
        }
        
        public void UpdateLogoUrl(Url newLogoUrl) => LogoUrl = newLogoUrl;
        
        public void UpdateBusinessEmail(Email newBusinessEmail) => BusinessEmail = newBusinessEmail;
        
        public void UpdateBusinessPhoneNumber(PhoneNumber newBusinessPhoneNumber) 
            => BusinessPhoneNumber = newBusinessPhoneNumber;
        
        public void UpdateAddress(Address newAddress) => Address = newAddress;
        
        public void UpdateCheckOutPolicy (CheckOutPolicy newCheckOutPolicy) 
            => CheckOutPolicy = newCheckOutPolicy;

        public void UpdateDefaultCurrencyCode(CurrencyCode newDefaultCurrencyCode)
        {
            if (Enum.IsDefined<CurrencyCode>(newDefaultCurrencyCode) is not true)
                throw new CurrencyNotSupportedException();
            
            DefaultCurrencyCode = newDefaultCurrencyCode;
        }

        public void UpdateItbisRate(ItbisRate newItbisRate) => ItbisRate = newItbisRate;

        public void UpdateServiceChargeRate(ServiceChargeRate newServiceChargeRate) 
            => ServiceChargeRate = newServiceChargeRate;

        public void SetAsActive() => IsActive = true;

        public void SetAsInactive() => IsActive = false;
    }
}
