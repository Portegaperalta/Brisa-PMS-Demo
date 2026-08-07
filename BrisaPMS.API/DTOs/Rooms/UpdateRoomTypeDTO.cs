using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Rooms;

public class UpdateRoomTypeDTO
{
    [Required(ErrorMessage = "The field {0} is required")]
    public required Guid RoomTypeId { get; set; }
}