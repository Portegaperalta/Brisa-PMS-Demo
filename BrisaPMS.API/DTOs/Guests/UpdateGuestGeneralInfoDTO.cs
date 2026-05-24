using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Guests;

public class UpdateGuestGeneralInfoDTO
{
    [Required(ErrorMessage = "The field {0} is required")]
    public required Guid GuestId { get; set; }
    [Required(ErrorMessage = "The field {0} is required")]
    public required string FirstName { get; set; }
    [Required(ErrorMessage = "The field {0} is required")]
    public required string LastName { get; set; }
    public string? Country { get; set; }
    public string? PreferredLanguage { get; set; }
    public string? Notes { get; set; }
}