using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Hotels
{
    public class UpdateAddressInfoDTO
    {
        [Required(ErrorMessage = "The field {0} is required")]
        public required string Address1 { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public string? Address2 { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public required string City { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public required string Province { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public required string ZipCode { get; set; }
    }
}