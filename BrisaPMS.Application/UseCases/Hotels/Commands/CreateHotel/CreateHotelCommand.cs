using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.CreateHotel;

public class CreateHotelCommand
{
    public required string LegalName { get; set; }
    public required string CommercialName { get; set; }
    public Url? LogoUrl { get; set; } = null;
    public required Email BusinessEmail { get; set; }
    public required PhoneNumber BusinessPhoneNumber { get; set; }
    public required Address Address { get; set; }
    public required CheckInOutTimes CheckInOutTimes { get; set; }
    public CurrencyCode DefaultCurrencyCode { get; set; }
    public required ItbisRate ItbisRate { get; set; }
    public required ServiceChargeRate ServiceChargeRate { get; set; }
    public required bool IsActive { get; set; }
}