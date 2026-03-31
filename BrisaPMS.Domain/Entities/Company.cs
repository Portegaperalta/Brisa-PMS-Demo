using System;
using BrisaPMS.Domain.Exceptions;
using BrisaPMS.Domain.ValueObjects;

namespace BrisaPMS.Domain.Entities;

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

    public void ChangeRnc(Rnc newRnc) => Rnc = newRnc;

    public void ChangeBusinessEmail(Email newBusinessEmail)  => BusinessEmail = newBusinessEmail;

    public void ChangeBusinessPhone(PhoneNumber newBusinessPhone) => BusinessPhone = newBusinessPhone;

    public void ChangeLogoUrl(Url newLogoUrl) => LogoUrl = newLogoUrl;

    public void ChangeAddress(Address newAddress) => Address = newAddress;
}