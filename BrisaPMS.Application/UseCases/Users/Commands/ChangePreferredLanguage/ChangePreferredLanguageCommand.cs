using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Users.Commands.ChangePreferredLanguage;

public class ChangePreferredLanguageCommand : IRequest<bool>
{
    public required Guid UserId { get; set; }
    public required string PreferredLanguage { get; set; }
}