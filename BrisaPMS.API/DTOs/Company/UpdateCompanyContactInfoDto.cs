namespace BrisaPMS.API.DTOs.Company;

public class UpdateCompanyContactInfoDto
{
    public required string NewBusinessEmail { get; set; }
    public required string NewBusinessPhoneNumber { get; set; }
}