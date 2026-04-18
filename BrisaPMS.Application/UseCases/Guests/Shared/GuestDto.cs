namespace BrisaPMS.Application.UseCases.Guests.Shared;

public class GuestDto
{
    public required Guid Id { get; init; }
    public required Guid HotelId { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string DocumentType { get; init; }
    public required string DocumentNumber { get; init; }
    public string? Country { get; init; }
    public string? Rnc { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
    public required string PreferredCurrency { get; init; }
    public string? PreferredLanguage { get; init; }
    public bool IsVip { get; init; }
    public bool IsBlackListed { get; init; }
    public string? BlackListedReason { get; init; }
    public string? Notes { get; init; }
}