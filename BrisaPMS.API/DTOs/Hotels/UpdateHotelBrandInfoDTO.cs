using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Hotels
{
    public class UpdateHotelBrandInfoDTO
    {
        [Required(ErrorMessage = "The field {0} is required")]
        public required string LegalName { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public required string CommercialName { get; set; }
        public string? LogoUrl { get; set; }
    }
}