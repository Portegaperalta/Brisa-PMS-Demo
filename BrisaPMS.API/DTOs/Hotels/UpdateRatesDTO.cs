using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Hotels
{
    public class UpdateRatesDTO
    {
        [Required(ErrorMessage = "The field {0} is required")]
        public required decimal ItbisRate { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public required decimal ServiceChargeRate { get; set; }
    }
}