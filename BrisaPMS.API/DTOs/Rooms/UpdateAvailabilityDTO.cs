using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Rooms;

public class UpdateAvailabilityDTO
{
    [Required(ErrorMessage = "The field {0} is required")]
    public required string AvailabilityStatus { get; set; }
}