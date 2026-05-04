using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Guests.Commands.DeleteGuest;

public class DeleteGuestCommand : IRequest<bool>
{
    public required Guid Id { get; set; }
}