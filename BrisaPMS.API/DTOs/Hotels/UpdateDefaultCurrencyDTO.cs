using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Hotels
{
    public class UpdateDefaultCurrencyDTO
    {
        [Required]
        public required string CurrencyCode { get; set; }
    }
}