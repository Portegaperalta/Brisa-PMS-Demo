using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.ReportIncident;

public class ReportIncidentCommand : IRequest<bool>
{
    public required Guid HouseKeepingTaskId { get; set; }
    public required string IncidentDescription { get; set; }
}