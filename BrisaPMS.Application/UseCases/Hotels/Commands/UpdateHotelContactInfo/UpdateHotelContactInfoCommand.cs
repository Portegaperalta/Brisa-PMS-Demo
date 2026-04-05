using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelContactInfo;

public class UpdateHotelContactInfoCommand
{
    public required Guid Id { get; set; }
    public required Email BusinessEmail { get; set; }
    public required PhoneNumber BusinessPhoneNumber { get; set; }
}