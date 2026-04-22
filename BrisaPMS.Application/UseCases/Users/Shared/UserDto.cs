namespace BrisaPMS.Application.UseCases.Users.Shared;

public class UserDto
{
    public required Guid Id { get; init; }
    public required string Role { get; init; }
    public Guid? HotelId { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public string? PhoneNumber { get; init; }
    public required string PreferredLanguage { get; init; }
}