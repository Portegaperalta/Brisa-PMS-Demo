using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.RoomTypes.Commands.DeleteRoomType;

public class DeleteRoomTypeCommand : IRequest<bool>
{
    public required Guid Id { get; set; }
}