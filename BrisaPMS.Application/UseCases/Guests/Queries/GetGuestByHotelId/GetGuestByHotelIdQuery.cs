using BrisaPMS.Application.UseCases.Guests.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Guests.Queries.GetGuestByHotelId;

public class GetGuestByHotelIdQuery : IRequest<GuestDto?>
{
    public required Guid HotelId { get; set; }
}