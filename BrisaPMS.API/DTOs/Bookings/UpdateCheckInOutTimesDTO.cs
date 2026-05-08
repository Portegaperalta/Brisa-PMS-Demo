using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Bookings
{
    public class UpdateCheckInOutTimesDTO
    {
        [Required(ErrorMessage = "The field {0} is required")]
        public required DateTime CheckInTime { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public required DateTime CheckOutTime { get; set; }
    }
}