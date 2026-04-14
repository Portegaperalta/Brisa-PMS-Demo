using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeBedsInfo;

public class UpdateRoomTypeBedsInfoCommand : IRequest<bool>
{
    public required Guid RoomTypeId { get; set; }
    public required string BedType { get; set; }
    public required int NumberOfBeds { get; set; }
}