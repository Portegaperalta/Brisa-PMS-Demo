using BrisaPMS.Application.UseCases.Amenities.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Amenities.Queries.GetAmenityById;

public class GetAmenityByIdQuery : IRequest<AmenityDto>
{
    public required Guid AmenityId { get; set; }
}