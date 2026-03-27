using System;
using BrisaPMS.Domain.Exceptions;
using BrisaPMS.Domain.ValueObjects;

namespace BrisaPMS.Domain.Entities;

public class Company
{
    // Attributes
    public Guid Id { get; private init; } =  Guid.CreateVersion7();
    public string LegalName {get; private set;}
    public string CommercialName {get; private set;}
    public string Rnc {get; private set;}
    public Email BusinessEmail { get; private set; }
    public string BusinessPhone { get; private set; }
    public string? LogoUrl  { get; private set; }
    public string Address1 { get; private set; }
    public string Address2 { get; private set; }
    public string City { get; private set; }
    public string Province { get; private set; }
    public string ZipCode {get; private set;}
    public DateTime? UpdatedAt { get; private set; }

    // Constructor
    public Company(string legalName, 
        string commercialName, 
        string rnc,
        Email businessEmail, 
        string businessPhone, 
        string logoUrl, 
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
        
        if (string.IsNullOrWhiteSpace(rnc) is true)
            throw new EmptyRncException();
        
        if (string.IsNullOrWhiteSpace(businessPhone) is true)
            throw new EmptyPhoneNumberException();
        
        if (string.IsNullOrWhiteSpace(address1) is true)
            throw new EmptyAddress1Exception();
        
        if (string.IsNullOrWhiteSpace(city) is true)
            throw new EmptyCityFieldException();
        
        if (string.IsNullOrWhiteSpace(province) is true)
            throw new EmptyProvinceFieldException();
        
        if (string.IsNullOrWhiteSpace(zipCode) is true)
            throw new EmptyZipCodeException();
        
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
        
        LegalName = newLegalName;
    }

    public void ChangeCommercialName(string newCommercialName)
    {
        if (string.IsNullOrWhiteSpace(newCommercialName))
            throw new EmptyCommercialNameException();
        
        CommercialName = newCommercialName;
    }

    public void ChangeRnc(string newRnc)
    {
        if (string.IsNullOrWhiteSpace(newRnc))
            throw new EmptyRncException();
        
        if (newRnc.Length > 11)
            throw new MaxCharacterLimitException(11, "Rnc");
        
        Rnc = newRnc;
    }

    public void ChangeBusinessEmail(Email newBusinessEmail)  => BusinessEmail = newBusinessEmail;

    public void ChangeBusinessPhone(string newBusinessPhone)
    {
        if (string.IsNullOrWhiteSpace(newBusinessPhone))
            throw new EmptyPhoneNumberException();
        
        BusinessPhone = newBusinessPhone;
    }

    public void ChangeLogoUrl(string? newLogoUrl)
    { 
        LogoUrl = newLogoUrl;
    }

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