using BrisaPMS.Application.UseCases.Rooms.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Rooms.Queries.GetAllRoomsByHotelId;

public class GetAllRoomsByHotelIdQuery : IRequest<List<RoomDto>>
{
    public required Guid HotelId { get; set; }
}