using BrisaPMS.Application.UseCases.Amenities.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Amenities.Commands.CreateAmenity;

public class CreateAmenityCommand : IRequest<AmenityDto>
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsActive { get; set; }
}