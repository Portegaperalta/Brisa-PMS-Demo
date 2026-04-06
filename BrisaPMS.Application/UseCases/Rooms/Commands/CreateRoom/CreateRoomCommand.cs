using BrisaPMS.Domain.Rooms;

namespace BrisaPMS.Application.UseCases.Rooms.Commands.CreateRoom;

public class CreateRoomCommand
{
    public required Guid HotelId { get; set; }
    public required Guid RoomTypeId { get; set; }
    public required string Number { get; set; }
    public required int Floor { get; set; }
    public required RoomAvailabilityStatus AvailabilityStatus { get; set; }
    public required RoomHygieneStatus HygieneStatus { get; set; }
}