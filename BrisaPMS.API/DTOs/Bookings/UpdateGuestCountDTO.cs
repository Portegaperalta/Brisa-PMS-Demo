using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Bookings
{
    public class UpdateGuestCountDTO
    {
        [Required(ErrorMessage = "The field {0} is required")]
        public required int NumberOfAdults { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public required int NumberOfChildren { get; set; }
    }
}