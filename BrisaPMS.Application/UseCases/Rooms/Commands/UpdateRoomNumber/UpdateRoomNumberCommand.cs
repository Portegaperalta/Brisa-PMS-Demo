using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Rooms.Commands.UpdateRoomNumber;

public class UpdateRoomNumberCommand : IRequest<bool>
{
    public required Guid RoomId { get; set; }
    public required string Number { get; set; }
}