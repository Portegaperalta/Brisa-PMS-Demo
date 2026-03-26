using System;
using BrisaPMS.Domain.Exceptions;

namespace BrisaPMS.Domain.Entities;

public class Company
{
    public Guid Id { get; private init; } =  Guid.NewGuid();
    public string LegalName {get; private set;}
    public string CommercialName {get; private set;}
    public string Rnc {get; private init;}
    public string BusinessEmail { get; private set; }
    public string BusinessPhone { get; private set; }
    public string LogoUrl  { get; private set; }
    public string Address1 { get; private set; }
    public string Address2 { get; private set; }
    public string City { get; private set; }
    public string Province { get; private set; }
    public string ZipCode {get; private set;}
    public DateTime? UpdatedAt { get; private set; }

    public Company(string legalName, 
        string commercialName, 
        string rnc,
        string businessEmail, 
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
        
        if (string.IsNullOrWhiteSpace(businessEmail) is true)
            throw new EmptyEmailException();
        
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
}