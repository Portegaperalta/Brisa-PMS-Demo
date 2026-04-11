using BrisaPMS.Application.UseCases.Rooms.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Rooms.Queries.GetRoomById;

public class GetRoomByIdQuery : IRequest<RoomDto>
{
    public required Guid RoomId { get; set; }
}