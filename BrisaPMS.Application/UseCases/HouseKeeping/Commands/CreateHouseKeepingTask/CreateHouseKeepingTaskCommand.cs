using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.CreateHouseKeepingTask;

public class CreateHouseKeepingTaskCommand : IRequest<Guid>
{
    public required Guid RoomId { get; set; }
    public required Guid AssignedTo { get; set; }
    public required Guid AssignedBy { get; set; }
    public required string HouseKeepingTaskType { get; set; }
    public required string TaskPriority { get; set; }
    public required DateTime ExpectedStartTime { get; set; }
    public required DateTime ExpectedEndTime { get; set; }
    public string? Notes { get; set; }
}