using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.UpdateHouseKeepingTaskNotes;

public class UpdateHouseKeepingTaskNotesCommand : IRequest<bool>
{
    public required Guid HouseKeepingTaskId { get; set; }
    public required string Notes { get; set; }
}