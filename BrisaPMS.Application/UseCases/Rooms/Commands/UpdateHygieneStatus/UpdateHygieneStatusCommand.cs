using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Rooms.Commands.UpdateHygieneStatus;

public class UpdateHygieneStatusCommand : IRequest<bool>
{
    public required Guid RoomId { get; set; }
    public required Guid UserId { get; set; }
    public required string HygieneStatus { get; set; }
}