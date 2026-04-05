using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelAddressInfo;

public class UpdateHotelAddressInfoCommand
{
    public required Guid Id {get; set;}
    public required Address Address {get; set;}
}