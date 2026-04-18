using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.ChangeAssignedRoom;

public class ChangeAssignedRoomCommand : IRequest<bool>
{
    public required Guid BookingId { get; set; }
    public required Guid RoomId { get; set; }
}