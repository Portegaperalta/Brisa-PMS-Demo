using BrisaPMS.Application.UseCases.Guests.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Guests.Commands.CreateGuest;

public class CreateGuestCommand : IRequest<GuestDto>
{
    public required Guid HotelId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string DocumentType { get; set; }
    public required string DocumentNumber { get; set; }
    public string? Country { get; set; }
    public string? Rnc { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string PreferredCurrency {get; set;}
    public string? PreferredLanguage { get; set; }
    public required bool IsVip  { get; set; }
    public string? Notes { get; set; }
}