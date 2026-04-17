using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Amenities.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Amenities.Queries.GetAmenityById;

public class GetAmenityByIdUseCase : IRequestHandler<GetAmenityByIdQuery, AmenityDto>
{
    private readonly IAmenitiesRepository _amenitiesRepository;

    public GetAmenityByIdUseCase(IAmenitiesRepository amenitiesRepository)
    {
        _amenitiesRepository = amenitiesRepository;
    }

    public async Task<AmenityDto> Handle(GetAmenityByIdQuery query)
    {
        var amenity = await _amenitiesRepository.GetById(query.AmenityId);

        if (amenity is null)
            throw new NotFoundException("Amenity", query.AmenityId);

        var amenityDto = amenity.ToDto();
        return amenityDto;
    }
}