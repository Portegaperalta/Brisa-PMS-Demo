using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Guests;

public class BlacklistGuestDTO
{
    [Required(ErrorMessage = "The field {0} is required")]
    public required string BlacklistedReason { get; set; }
}