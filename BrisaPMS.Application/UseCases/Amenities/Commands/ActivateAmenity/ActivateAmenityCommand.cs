using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Amenities.Commands.ActivateAmenity;

public class ActivateAmenityCommand : IRequest<bool>
{
    public required Guid AmenityId { get; set; }
}