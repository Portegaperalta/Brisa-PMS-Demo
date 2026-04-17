using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Amenities.Commands.DeactivateAmenity;

public class DeactivateAmenityCommand : IRequest<bool>
{
  public required Guid AmenityId { get; set; }
}