using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.CompleteHouseKeepingTask;

public class CompleteHouseKeepingTaskCommand : IRequest<bool>
{
    public required Guid HouseKeepingTaskId { get; set; }
}