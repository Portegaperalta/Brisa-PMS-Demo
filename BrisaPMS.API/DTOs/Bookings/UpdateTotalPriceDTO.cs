using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Bookings
{
    public class UpdateTotalPriceDTO
    {
        [Required(ErrorMessage = "The field {0} is required")]
        public required decimal TotalPrice { get; set; }
    }
}