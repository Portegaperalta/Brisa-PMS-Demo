using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Rooms.Commands.SetAsPendingRestock;

public class SetAsPendingRestockCommand : IRequest<bool>
{
    public required Guid RoomId { get; set; }
}