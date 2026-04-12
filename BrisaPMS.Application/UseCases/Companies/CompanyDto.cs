namespace BrisaPMS.Application.UseCases.Companies;

public class CompanyDto
{
    public required Guid Id { get; init; }
    public required string LegalName { get; init; }
    public required string CommercialName { get; init; }
    public required string Rnc {get; init; }
    public required string Email {get; init; }
    public required string BusinessPhone {get; init; }
    public string? LogoUrl  {get; init; }
    public required string Address1 {get; init; }
    public string? Address2 {get; init; }
    public required string City {get; init; }
    public required string Province {get; init; }
    public required string ZipCode {get; init; }
}