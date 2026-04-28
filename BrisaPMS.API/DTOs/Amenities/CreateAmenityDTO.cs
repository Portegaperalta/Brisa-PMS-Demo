using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Amenities
{
    public class CreateAmenityDTO
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [StringLength(500)]
        public required string Description { get; set; }

        public bool IsActive { get; set; }
    }
}