using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotel;

public class UpdateHotelBrandInfoCommand
{
    public required Guid Id { get; set; }
    public required string LegalName {get; set;}
    public required string CommercialName { get; set; }
    public required Url? LogoUrl { get; set; } = null;
}