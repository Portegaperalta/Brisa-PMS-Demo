using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.RoomTypes.Commands.CreateRoomType;

public class CreateRoomTypeCommand : IRequest<Guid>
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required decimal BaseRate { get; set; }
    public required int TotalBeds { get; set; }
    public required string BedType { get; set; }
    public required int MaxOccupancyAdults { get; set; }
    public required int MaxOccupancyChildren { get; set; }
}