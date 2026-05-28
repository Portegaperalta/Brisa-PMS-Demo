using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.RoomTypes;

public class UpdateRoomTypeOccupancyPolicyDTO
{
    [Required(ErrorMessage = "The field {0} is required")]
    public required int MaxOccupancyAdults { get; set; }
    [Required(ErrorMessage = "The field {0} is required")]
    public required int MaxOccupancyChildren { get; set; }
}