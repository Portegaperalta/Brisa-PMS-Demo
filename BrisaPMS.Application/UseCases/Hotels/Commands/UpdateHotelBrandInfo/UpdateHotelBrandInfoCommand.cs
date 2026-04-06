namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelBrandInfo;

public class UpdateHotelBrandInfoCommand
{
    public required Guid Id { get; set; }
    public required string LegalName { get; set; }
    public required string CommercialName { get; set; }
    public string? LogoUrl { get; set; } 
}