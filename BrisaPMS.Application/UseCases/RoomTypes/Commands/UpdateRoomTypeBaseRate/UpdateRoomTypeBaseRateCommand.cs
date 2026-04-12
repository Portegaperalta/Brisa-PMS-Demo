using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeBaseRate;

public class UpdateRoomTypeBaseRateCommand : IRequest<bool>
{
    public required Guid RoomTypeId { get; set; }
    public required decimal NewBaseRate { get; set; }
}