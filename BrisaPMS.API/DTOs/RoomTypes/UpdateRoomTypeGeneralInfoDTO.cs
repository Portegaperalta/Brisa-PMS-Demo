using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.RoomTypes;

public class UpdateRoomTypeGeneralInfoDTO
{
    [Required(ErrorMessage = "The field {0} is required")]
    public required string Name { get; set; }
    public string? Description { get; set; }
}