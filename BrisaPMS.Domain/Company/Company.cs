using System;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Domain.Company;

public class Company
{
    // Attributes
    public Guid Id { get; init; }
    public string LegalName {get; private set;}
    public string CommercialName {get; private set;}
    public Rnc Rnc {get; private set;}
    public Email BusinessEmail { get; private set; }
    public PhoneNumber BusinessPhone { get; private set; }
    public Url? LogoUrl  { get; private set; }
    public Address Address { get; private set; }

    // Constructor
    public Company
        (
            string legalName,
            string commercialName,
            Rnc rnc, Email businessEmail,
            PhoneNumber businessPhone,
            Url logoUrl,
            Address address
        ) 
    {
        if (string.IsNullOrWhiteSpace(legalName) is true)
            throw new EmptyRequiredFieldException("Legal Name");

        if (string.IsNullOrWhiteSpace(commercialName) is true)
            throw new EmptyRequiredFieldException("Commercial Name");
        
        Id = Guid.CreateVersion7();
        LegalName = legalName;
        CommercialName = commercialName;
        Rnc = rnc;
        BusinessEmail = businessEmail;
        BusinessPhone = businessPhone;
        LogoUrl = logoUrl;
        Address = address;
    }

    //Behavioral methods
    public void ChangeLegalName(string newLegalName)
    {
        if (string.IsNullOrWhiteSpace(newLegalName))
            throw new EmptyRequiredFieldException("Legal Name");

        LegalName = newLegalName;
    }

    public void ChangeCommercialName(string newCommercialName)
    {
        if (string.IsNullOrWhiteSpace(newCommercialName))
            throw new EmptyRequiredFieldException("Commercial Name");

        CommercialName = newCommercialName;
    }

    public void UpdateRnc(Rnc newRnc) => Rnc = newRnc;

    public void UpdateBusinessEmail(Email newBusinessEmail)  => BusinessEmail = newBusinessEmail;

    public void UpdateBusinessPhone(PhoneNumber newBusinessPhone) => BusinessPhone = newBusinessPhone;

    public void UpdateLogoUrl(Url newLogoUrl) => LogoUrl = newLogoUrl;

    public void UpdateAddress(Address newAddress) => Address = newAddress;
}