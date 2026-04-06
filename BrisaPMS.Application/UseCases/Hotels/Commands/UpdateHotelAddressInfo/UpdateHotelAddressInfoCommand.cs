using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelAddressInfo;

public class UpdateHotelAddressInfoCommand
{
    public required Guid HotelId {get; set;}
    public required string Address1 {get; set;}
    public string? Address2 {get; set;}
    public required string City {get; set;}
    public required string Province {get; set;}
    public required string ZipCode {get; set;}
}