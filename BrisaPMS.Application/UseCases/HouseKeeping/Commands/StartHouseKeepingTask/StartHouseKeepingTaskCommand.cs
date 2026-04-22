using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.StartHouseKeepingTask;

public class StartHouseKeepingTaskCommand : IRequest<bool>
{
    public required Guid HouseKeepingTaskId { get; set; }
}