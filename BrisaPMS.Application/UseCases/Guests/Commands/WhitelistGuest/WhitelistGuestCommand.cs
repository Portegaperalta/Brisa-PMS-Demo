using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Guests.Commands.WhitelistGuest;

public class WhitelistGuestCommand : IRequest<bool>
{
    public required Guid GuestId { get; set; }
}