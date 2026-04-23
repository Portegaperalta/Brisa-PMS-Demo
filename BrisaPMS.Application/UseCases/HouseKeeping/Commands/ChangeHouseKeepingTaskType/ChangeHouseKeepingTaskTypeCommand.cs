using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.ChangeHouseKeepingTaskType;

public class ChangeHouseKeepingTaskTypeCommand : IRequest<bool>
{
    public required Guid HouseKeepingTaskId { get; set; }
    public required string HouseKeepingTaskType { get; set; }
}