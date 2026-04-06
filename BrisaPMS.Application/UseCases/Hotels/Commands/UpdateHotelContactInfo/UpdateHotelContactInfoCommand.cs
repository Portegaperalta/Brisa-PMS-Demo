using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelContactInfo;

public class UpdateHotelContactInfoCommand
{
    public required Guid HotelId { get; set; }
    public required string BusinessEmail { get; set; }
    public required string BusinessPhoneNumber { get; set; }
}