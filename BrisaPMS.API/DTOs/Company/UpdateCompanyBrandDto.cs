namespace BrisaPMS.API.DTOs.Company;

public class UpdateCompanyBrandDto
{
    public required string NewLegalName { get; set; }
    public required string NewCommercialName { get; set; }
    public string? NewLogoUrl { get; set; }
}