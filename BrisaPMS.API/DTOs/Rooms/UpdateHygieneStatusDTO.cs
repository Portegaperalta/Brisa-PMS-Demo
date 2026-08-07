using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Rooms;

public class UpdateHygieneStatusDTO
{
    [Required(ErrorMessage = "The field {0} is required")]
    public required string HygieneStatus { get; set; }
}