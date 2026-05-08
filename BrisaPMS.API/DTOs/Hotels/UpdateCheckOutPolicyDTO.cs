using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Hotels
{
    public class UpdateCheckOutPolicyDTO
    {
        [Required(ErrorMessage = "The field {0} is required")]
        public required TimeOnly CheckInTime { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public required TimeOnly CheckOutTime { get; set; }
    }
}