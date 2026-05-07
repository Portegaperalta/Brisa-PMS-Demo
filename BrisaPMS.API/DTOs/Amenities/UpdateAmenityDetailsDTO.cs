using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Amenities
{
    public class UpdateAmenityDetailsDTO
    {
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(500)]
        public required string Description { get; set; }
    }
}