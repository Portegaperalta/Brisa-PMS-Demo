using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Users.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<bool>
{
    public required Guid UserId { get; set; }
    public required string Password { get; set; }
}