using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Amenities.Commands.UpdateAmenityDetails;

public class UpdateAmenityDetailsCommand : IRequest<bool>
{
    public required Guid AmenityId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}