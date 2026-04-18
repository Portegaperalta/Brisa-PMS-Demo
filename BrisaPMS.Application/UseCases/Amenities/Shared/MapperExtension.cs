using BrisaPMS.Domain.Amenities;

namespace BrisaPMS.Application.UseCases.Amenities.Shared;

public static class MapperExtension
{
    public static AmenityDto ToDto(this Amenity amenity)
    {
        return new AmenityDto
        {
            Id = amenity.Id,
            Name = amenity.Name,
            Description = amenity.Description,
            IsActive = amenity.IsActive,
        };
    }
}