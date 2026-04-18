using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestRnc;

public class UpdateGuestRncCommand : IRequest<bool>
{
    public required Guid GuestId { get; set; }
    public required string Rnc { get; set; }
}