using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Rooms.Commands.SetAsRestocked;

public class SetAsRestockedCommand : IRequest<bool>
{
    public required Guid RoomId { get; set; }
}