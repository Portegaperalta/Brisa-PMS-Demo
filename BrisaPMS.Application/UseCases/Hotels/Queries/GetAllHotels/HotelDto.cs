namespace BrisaPMS.Application.UseCases.Hotels.Queries.GetAllHotels;

public class HotelDto
{
    public Guid Id { get; set; }
    public string LegalName { get; set; }
    public string CommercialName { get; set; }
    public string? LogoUrl { get; set; }
    public string BusinessEmail { get; set; }
    public string BusinessPhoneNumber { get; set; }
    public string Address1 { get; set; }
    public string? Address2 { get; set; }
    public string City { get; set; }
    public string Province { get; set; }
    public string ZipCode { get; set; }
    public TimeOnly CheckInTime { get; set; }
    public TimeOnly CheckOutTime { get; set; }
    public string DefaultCurrencyCode { get; set; }
    public decimal ItbisRate { get; set; }
    public decimal ServiceChargeRate { get; set; }
    public bool IsActive { get; set; }
}