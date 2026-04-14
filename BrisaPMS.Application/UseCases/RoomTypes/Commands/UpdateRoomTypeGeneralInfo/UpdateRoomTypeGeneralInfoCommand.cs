using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeGeneralInfo;

public class UpdateRoomTypeGeneralInfoCommand : IRequest<bool>
{
    public required Guid RoomTypeId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}