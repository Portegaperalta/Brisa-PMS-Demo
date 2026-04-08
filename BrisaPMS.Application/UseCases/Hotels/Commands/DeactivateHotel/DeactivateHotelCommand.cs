using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.DeactivateHotel;

public class DeactivateHotelCommand  : IRequest<bool>
{
    public required Guid HotelId { get; set; }
}