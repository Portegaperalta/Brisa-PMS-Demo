using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Guests;

public class UpdateGuestContactInfoDTO
{
    [Required(ErrorMessage = "The field {0} is required")]
    [EmailAddress]
    public required string Email { get; set; }
    [Required(ErrorMessage = "The field {0} is required")]
    public required string PhoneNumber { get; set; }
}