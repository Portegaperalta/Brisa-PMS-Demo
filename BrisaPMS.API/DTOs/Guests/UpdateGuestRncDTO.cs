using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Guests;

public class UpdateGuestRncDTO
{
    [Required(ErrorMessage = "The field {0} is required")]
    public required string Rnc {get; set;}
}