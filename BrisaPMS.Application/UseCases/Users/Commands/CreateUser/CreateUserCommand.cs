using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<Guid>
{
    public required string Role { get; set; }
    public Guid? HotelId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? PhoneNumber { get; set; }
    public required string PreferredLanguage { get; set; }
}