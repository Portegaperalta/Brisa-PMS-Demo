using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Hotels
{
    public class UpdateHotelContactInfoDTO
    {
        [Required(ErrorMessage = "The field {0} is required")]
        [EmailAddress]
        public required string BusinessEmail { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public required string BusinessPhoneNumber { get; set; }
    }
}