using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.RoomTypes;

public class UpdateRoomTypeBedsInfoDTO
{
    [Required(ErrorMessage = "The field {0} is required")]
    public required string BedType { get; set; }
    [Required(ErrorMessage = "The field {0} is required")]
    public required int NumberOfBeds { get; set; }
}