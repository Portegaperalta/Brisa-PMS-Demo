using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.ChangeTaskDeadline;

public class ChangeTaskDeadlineCommand : IRequest<bool>
{
    public required Guid HouseKeepingTaskId { get; set; }
    public required DateTime ExpectedStartTime { get; set; }
    public required DateTime ExpectedEndTime { get; set; }
}