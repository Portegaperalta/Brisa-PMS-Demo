using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Users
{
    public class CreateUserDTO
    {
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(25)]
        public required string Role { get; set; }

        public Guid? HotelId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(250)]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(250)]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(254)]
        [EmailAddress]
        public required string Email { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(512, MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$",
        ErrorMessage = "Password must contain at least one uppercase letter, one number, and one special character.")]
        public required string Password { get; set; }

        [StringLength(25)]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Must be a valid phone number.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(5)]
        public required string PreferredLanguage { get; set; }
    }
}