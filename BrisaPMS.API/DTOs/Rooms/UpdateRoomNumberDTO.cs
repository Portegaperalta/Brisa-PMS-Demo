using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Rooms;

public class UpdateRoomNumberDTO
{
    [Required(ErrorMessage = "The field {0} is required")]
    public required string RoomNumber { get; set; }
}