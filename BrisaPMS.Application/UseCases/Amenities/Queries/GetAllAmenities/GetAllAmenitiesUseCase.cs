using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.UseCases.Amenities.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Amenities.Queries.GetAllAmenities;

public class GetAllAmenitiesUseCase : IRequestHandler<GetAllAmenitiesQuery, List<AmenityDto>>
{
    private readonly IAmenitiesRepository _amenitiesRepository;

    public GetAllAmenitiesUseCase(IAmenitiesRepository amenitiesRepository)
    {
        _amenitiesRepository = amenitiesRepository;
    }

    public async Task<List<AmenityDto>> Handle(GetAllAmenitiesQuery query)
    {
        var amenities = await _amenitiesRepository.GetAll();
        var amenitiesDtos = amenities.Select(a => a.ToDto()).ToList();
        return amenitiesDtos;
    }
}