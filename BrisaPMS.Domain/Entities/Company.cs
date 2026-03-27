using System;
using BrisaPMS.Domain.Exceptions;
using BrisaPMS.Domain.Exceptions.EmptyValueExceptions;
using BrisaPMS.Domain.ValueObjects;

namespace BrisaPMS.Domain.Entities;

public class Company
{
    // Attributes
    public Guid Id { get; private init; } =  Guid.CreateVersion7();
    public string LegalName {get; private set;}
    public string CommercialName {get; private set;}
    public Rnc Rnc {get; private set;}
    public Email BusinessEmail { get; private set; }
    public PhoneNumber BusinessPhone { get; private set; }
    public Url? LogoUrl  { get; private set; }
    public string Address1 { get; private set; }
    public string Address2 { get; private set; }
    public string City { get; private set; }
    public string Province { get; private set; }
    public string ZipCode {get; private set;}
    public DateTime? UpdatedAt { get; private set; }

    private readonly int MaxLegalNameLength = 250;
    private readonly int MaxCommercialNameLength = 250;

    // Constructor
    public Company(string legalName, 
        string commercialName, 
        Rnc rnc,
        Email businessEmail, 
        PhoneNumber businessPhone, 
        Url logoUrl, 
        string address1,
        string address2, 
        string city, 
        string province, 
        string zipCode)
    {
        if (string.IsNullOrEmpty(legalName) is true)
            throw new EmptyLegalNameException();
        
        if (string.IsNullOrWhiteSpace(commercialName) is true)
            throw new EmptyCommercialNameException();
        
        if (string.IsNullOrWhiteSpace(address1) is true)
            throw new EmptyAddress1Exception();
        
        if (string.IsNullOrWhiteSpace(city) is true)
            throw new EmptyCityFieldException();
        
        if (string.IsNullOrWhiteSpace(province) is true)
            throw new EmptyProvinceFieldException();
        
        if (string.IsNullOrWhiteSpace(zipCode) is true)
            throw new EmptyZipCodeException();

        if (legalName.Length > MaxLegalNameLength)
            throw new MaxCharacterLimitException(MaxLegalNameLength, "Legal Name");

        if (commercialName.Length > MaxCommercialNameLength)
            throw new MaxCharacterLimitException(MaxCommercialNameLength, "Commercial Name");
        
        LegalName = legalName;
        CommercialName = commercialName;
        Rnc = rnc;
        BusinessEmail = businessEmail;
        BusinessPhone = businessPhone;
        LogoUrl = logoUrl;
        Address1 = address1;
        Address2 = address2;
        City = city;
        Province = province;
        ZipCode = zipCode;
        UpdatedAt = null;
    }

    //Behavioral methods
    public void ChangeLegalName(string newLegalName)
    {
        if (string.IsNullOrWhiteSpace(newLegalName))
            throw new EmptyLegalNameException();

        if (newLegalName.Length > MaxLegalNameLength)
            throw new MaxCharacterLimitException(MaxLegalNameLength, "Legal Name");

        LegalName = newLegalName;
    }

    public void ChangeCommercialName(string newCommercialName)
    {
        if (string.IsNullOrWhiteSpace(newCommercialName))
            throw new EmptyCommercialNameException();

        if (newCommercialName.Length > MaxCommercialNameLength)
            throw new MaxCharacterLimitException(MaxCommercialNameLength, "Commercial Name");

        CommercialName = newCommercialName;
    }

    public void ChangeRnc(Rnc newRnc) => Rnc = newRnc;

    public void ChangeBusinessEmail(Email newBusinessEmail)  => BusinessEmail = newBusinessEmail;

    public void ChangeBusinessPhone(PhoneNumber newBusinessPhone) => BusinessPhone = newBusinessPhone;

    public void ChangeLogoUrl(Url newLogoUrl) => LogoUrl = newLogoUrl;

    public void ChangeAddress1(string newAddress1)
    {
        if (string.IsNullOrWhiteSpace(newAddress1))
            throw new EmptyAddress1Exception();
        
        Address1 = newAddress1;
    }
    
    public void ChangeAddress2(string newAddress2) => Address2 = newAddress2;

    public void ChangeCity(string newCity)
    {
        if (string.IsNullOrWhiteSpace(newCity))
            throw new EmptyCityFieldException();
        
        City = newCity;
    }

    public void ChangeProvince(string newProvince)
    {
        if (string.IsNullOrWhiteSpace(newProvince))
            throw new EmptyProvinceFieldException();
        
        Province = newProvince;
    }

    public void ChangeZipCode(string newZipCode)
    {
        if (string.IsNullOrWhiteSpace(newZipCode))
            throw new EmptyZipCodeException();
        
        ZipCode = newZipCode;
    }
    
    public void UpdateLastProfileUpdateTime() => UpdatedAt = DateTime.UtcNow;
}