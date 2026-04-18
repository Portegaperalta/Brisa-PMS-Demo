using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestGeneralInfo;

public class UpdateGuestGeneralInfoCommand : IRequest<bool>
{
    public Guid GuestId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Country { get; set; }
    public string? PreferredLanguage { get; set; }
    public string? Notes { get; set; }
}