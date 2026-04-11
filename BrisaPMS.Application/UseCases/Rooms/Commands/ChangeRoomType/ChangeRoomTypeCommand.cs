using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Rooms.Commands.ChangeRoomType;

public class ChangeRoomTypeCommand : IRequest<bool>
{
    public required Guid RoomId { get; set; }
    public required Guid RoomTypeId { get; set; }
}