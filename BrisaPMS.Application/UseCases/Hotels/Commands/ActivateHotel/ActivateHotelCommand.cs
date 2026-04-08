using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.ActivateHotel;

public class ActivateHotelCommand : IRequest<bool>
{
    public required Guid HotelId { get; set; }
}