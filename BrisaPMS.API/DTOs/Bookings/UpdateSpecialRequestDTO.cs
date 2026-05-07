using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Bookings
{
    public class UpdateSpecialRequestDTO
    {
        [Required(ErrorMessage = "The field {0} is required")]
        public required string SpecialRequest { get; set; }
    }
}