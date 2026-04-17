using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Guests.Commands.BlacklistGuest;

public class BlacklistGuestCommand : IRequest<bool>
{
    public required Guid GuestId { get; set; }
    public required string BlacklistedReason { get; set; }
}