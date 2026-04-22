using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.UpdateHouseKeepingTaskPriority;

public class UpdateHouseKeepingTaskPriorityCommand : IRequest<bool>
{
    public required Guid HouseKeepingTaskId { get; set; }
    public required string TaskPriority { get; set; }
}