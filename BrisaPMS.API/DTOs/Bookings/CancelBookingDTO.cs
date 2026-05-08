using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Bookings
{
    public class CancelBookingDTO
    {
        [Required(ErrorMessage = "The field {0} is required")]
        public required string CancellationReason { get; set; }
    }
}