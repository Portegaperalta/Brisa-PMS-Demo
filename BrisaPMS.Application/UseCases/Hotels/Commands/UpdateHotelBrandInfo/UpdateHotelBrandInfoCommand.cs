using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelBrandInfo;

public class UpdateHotelBrandInfoCommand
{
    public required Guid Id { get; set; }
    public required string LegalName { get; set; }
    public required string CommercialName { get; set; }
    public Url? LogoUrl { get; set; } 
}