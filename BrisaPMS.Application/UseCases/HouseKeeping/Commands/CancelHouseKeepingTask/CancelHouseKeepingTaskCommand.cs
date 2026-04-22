using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.CancelHouseKeepingTask;

public class CancelHouseKeepingTaskCommand : IRequest<bool>
{
    public required Guid HouseKeepingTaskId { get; set; }
}