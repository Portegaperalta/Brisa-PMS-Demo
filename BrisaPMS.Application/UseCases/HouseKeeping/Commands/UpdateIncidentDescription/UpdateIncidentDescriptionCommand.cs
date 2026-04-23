using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.UpdateIncidentDescription;

public class UpdateIncidentDescriptionCommand : IRequest<bool>
{
    public required Guid HouseKeepingTaskId { get; set; }
    public required string IncidentDescription { get; set; }
}