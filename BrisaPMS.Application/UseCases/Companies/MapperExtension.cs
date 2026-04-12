using BrisaPMS.Domain.Companies;

namespace BrisaPMS.Application.UseCases.Companies;

public static class MapperExtension
{
    public static CompanyDto ToDto(this Company company)
    {
        return new CompanyDto
        {
            Id = company.Id,
            LegalName = company.LegalName,
            CommercialName = company.CommercialName,
            Rnc = company.Rnc.Value,
            Email = company.BusinessEmail.Value,
            BusinessPhone = company.BusinessPhone.Value,
            LogoUrl = company.LogoUrl!.Value,
            Address1 = company.Address.Address1,
            Address2 = company.Address.Address2,
            City = company.Address.City,
            Province = company.Address.Province,
            ZipCode = company.Address.ZipCode,
        };
    }
}