namespace BrisaPMS.Application.UseCases.Hotels.Shared;

public class HotelDto
{
    public Guid Id { get; init; }
    public required string LegalName { get; init; }
    public required string CommercialName { get; init; }
    public string? LogoUrl { get; init; }
    public required string BusinessEmail { get; init; }
    public required string BusinessPhoneNumber { get; init; }
    public required string Address1 { get; init; }
    public string? Address2 { get; init; }
    public required string City { get; init;}
    public required string Province { get; init; }
    public required string ZipCode { get; init; }
    public required TimeOnly CheckInTime { get; init; }
    public required TimeOnly CheckOutTime { get; init; }
    public required string DefaultCurrencyCode { get; init; }
    public required decimal ItbisRate { get; init; }
    public required decimal ServiceChargeRate { get; init; }
    public required bool IsActive { get; init;}
}