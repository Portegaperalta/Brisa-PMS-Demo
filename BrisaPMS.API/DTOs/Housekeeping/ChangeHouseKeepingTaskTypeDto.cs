using System.ComponentModel.DataAnnotations;

namespace BrisaPMS.API.DTOs.Housekeeping;

public class ChangeHouseKeepingTaskTypeDto
{
    [Required]
    public required string HouseKeepingTaskType { get; set; }
}