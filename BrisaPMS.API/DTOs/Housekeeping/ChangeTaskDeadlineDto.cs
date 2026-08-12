using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Housekeeping;

public class ChangeTaskDeadlineDto
{
    [Required]
    public required DateTime ExpectedStartTime { get; set; }
    
    [Required]
    public required DateTime ExpectedEndTime { get; set; }
}