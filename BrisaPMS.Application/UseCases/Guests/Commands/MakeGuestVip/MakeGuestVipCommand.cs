using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Guests.Commands.MakeGuestVip;

public class MakeGuestVipCommand : IRequest<bool>
{
    public required Guid  GuestId { get; set; }
}