using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Users.Commands.ChangePhoneNumber;

public class ChangePhoneNumberCommand : IRequest<bool>
{
    public required Guid UserId { get; set; }
    public required string PhoneNumber { get; set; }
}