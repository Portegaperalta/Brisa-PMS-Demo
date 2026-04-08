using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.CreateHotel;

public class CreateHotelCommand : IRequest<Guid>
{
    public required string LegalName { get; set; }
    public required string CommercialName { get; set; }
    public string? LogoUrl { get; set; } = null;
    public required string BusinessEmail { get; set; }
    public required string BusinessPhoneNumber { get; set; }
    public required string Address1 { get; set; }
    public string? Address2 { get; set; }
    public required string City { get; set; }
    public required string Province { get; set; }
    public required string ZipCode { get; set; }
    public required TimeOnly CheckInTime { get; set; }
    public required TimeOnly CheckOutTime { get; set; }
    public string DefaultCurrencyCode { get; set; } = "DOP";
    public required decimal ItbisRate { get; set; }
    public required decimal ServiceChargeRate { get; set; }
    public bool IsActive { get; set; }
}