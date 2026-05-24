using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Guests;

public class UpdateGuestDocumentationDTO
{
    [Required(ErrorMessage = "The field {0} is required")]
    public required string DocumentType  { get; set; }
    [Required(ErrorMessage = "The field {0} is required")]
    public required string DocumentNumber {get; set; }
}