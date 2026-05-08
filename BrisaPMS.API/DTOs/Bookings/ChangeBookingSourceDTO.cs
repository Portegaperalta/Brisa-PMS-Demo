using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Bookings
{
    public class ChangeBookingSourceDTO
    {
        [Required]
        public required string BookingSource { get; set; }
    }
}