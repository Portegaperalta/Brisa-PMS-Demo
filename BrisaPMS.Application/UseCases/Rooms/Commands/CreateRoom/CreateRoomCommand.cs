using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Rooms;

namespace BrisaPMS.Application.UseCases.Rooms.Commands.CreateRoom;

public class CreateRoomCommand : IRequest<Guid>
{
    public required Guid HotelId { get; set; }
    public required Guid RoomTypeId { get; set; }
    public required string Number { get; set; }
    public required int Floor { get; set; }
    public required string AvailabilityStatus { get; set; }
    public required string HygieneStatus { get; set; }
}