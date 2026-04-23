using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.ReassignHouseKeepingTask;

public class ReassignHouseKeepingTaskCommand : IRequest<bool>
{
    public required Guid HouseKeepingTaskId { get; set; }
    public required Guid AssignedTo { get; set; }
}