using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Guests.Commands.RevokeGuestVip;

public class RevokeGuestVipCommand : IRequest<bool>
{
  public required Guid GuestId { get; set; }
}