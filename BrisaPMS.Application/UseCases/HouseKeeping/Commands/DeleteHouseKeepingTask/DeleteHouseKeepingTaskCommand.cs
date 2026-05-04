using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.DeleteHouseKeepingTask;

public class DeleteHouseKeepingTaskCommand : IRequest<bool>
{
    public required Guid Id { get; set; }
}