namespace BrisaPMS.API.DTOs.Company;

public class UpdateCompanyAddressDto
{
    public required string NewAddress1 { get; set; }
    public string? NewAddress2 { get; set; }
    public required string NewCity { get; set; }
    public required string NewProvince { get; set; }
    public required string NewZipCode { get; set; }
}