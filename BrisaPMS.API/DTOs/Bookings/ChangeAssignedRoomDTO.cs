using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Bookings
{
    public class ChangeAssignedRoomDTO
    {
        [Required(ErrorMessage = "The field {0} is required")]
        public required Guid RoomId { get; set; }
    }
}