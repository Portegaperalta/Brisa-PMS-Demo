using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Users.Commands.ChangeRole;

public class ChangeRoleCommand : IRequest<bool>
{
    public required Guid UserId { get; set; }
    public required string Role { get; set; }
}