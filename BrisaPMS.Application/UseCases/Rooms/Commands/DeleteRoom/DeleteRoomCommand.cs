using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Rooms.Commands.DeleteRoom;

public class DeleteRoomCommand : IRequest<bool>
{
    public required Guid Id { get; set; }
}