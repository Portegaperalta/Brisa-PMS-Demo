using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Amenities
{
    public class UpdateAmenityDetailsDTO
    {
        [Required]
        public required Guid AmenityId { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [StringLength(500)]
        public required string Description { get; set; }
    }
}
