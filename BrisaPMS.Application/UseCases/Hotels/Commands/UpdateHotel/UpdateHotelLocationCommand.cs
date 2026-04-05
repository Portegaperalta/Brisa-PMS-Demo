using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotel;

public class UpdateHotelLocationCommand
{
    public required Guid Id { get; set; }
    public required Address Address { get; set; }
}