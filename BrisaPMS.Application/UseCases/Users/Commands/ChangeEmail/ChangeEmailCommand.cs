using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Users.Commands.ChangeEmail;

public class ChangeEmailCommand : IRequest<bool>
{
    public required Guid UserId { get; set; }
    public required string Email { get; set; }
}