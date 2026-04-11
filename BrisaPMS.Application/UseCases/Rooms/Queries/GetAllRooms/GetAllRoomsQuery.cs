using BrisaPMS.Application.UseCases.Rooms.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Rooms.Queries.GetAllRooms;

public class GetAllRoomsQuery : IRequest<List<RoomDto>>
{
}