using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestDocumentation;

public class UpdateGuestDocumentationCommand : IRequest<bool>
{
    public required Guid GuestId { get; set; }
    public required string DocumentType {get; set;}
    public required string DocumentNumber {get; set;}
}