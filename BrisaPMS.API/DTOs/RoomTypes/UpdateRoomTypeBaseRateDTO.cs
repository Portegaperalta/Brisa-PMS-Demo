using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.RoomTypes;

public class UpdateRoomTypeBaseRateDTO
{
    [Required(ErrorMessage = "The field {0} is required")]
    public required decimal NewBaseRate { get; set; }
}