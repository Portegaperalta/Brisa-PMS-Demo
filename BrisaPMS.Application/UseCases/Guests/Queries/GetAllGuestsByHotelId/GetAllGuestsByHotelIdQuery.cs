using BrisaPMS.Application.UseCases.Guests.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Guests.Queries.GetAllGuestsByHotelId;

public class GetAllGuestsByHotelIdQuery : IRequest<List<GuestDto>>
{
    public required Guid HotelId { get; init; }
}