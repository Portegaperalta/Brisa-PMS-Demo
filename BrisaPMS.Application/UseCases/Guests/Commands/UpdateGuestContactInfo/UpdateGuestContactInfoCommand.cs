using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestContactInfo;

public class UpdateGuestContactInfoCommand : IRequest<bool>
{
  public required Guid GuestId { get; set; }
  public required string Email { get; set; }
  public required string PhoneNumber { get; set; }
}