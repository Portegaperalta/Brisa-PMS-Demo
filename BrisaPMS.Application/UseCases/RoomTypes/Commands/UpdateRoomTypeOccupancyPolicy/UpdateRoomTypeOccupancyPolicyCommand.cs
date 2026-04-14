using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeOccupancyPolicy;

public class UpdateRoomTypeOccupancyPolicyCommand : IRequest<bool>
{
    public required Guid RoomTypeId { get; set; }
    public required int MaxOccupancyAdults { get; set; }
    public required int MaxOccupancyChildren { get; set; }
}