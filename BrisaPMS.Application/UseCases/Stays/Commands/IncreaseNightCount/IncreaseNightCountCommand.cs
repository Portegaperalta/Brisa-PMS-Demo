using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Stays.Commands.IncreaseNightCount;

public class IncreaseNightCountCommand : IRequest<bool>
{
    public required Guid StayId { get; set; }
}