using BrisaPMS.Domain.RoomTypes;

namespace BrisaPMS.Application.UseCases.RoomTypes.Shared;

public static class MapperExtension
{
    public static RoomTypeDto ToDto(this RoomType roomType)
    {
        return new RoomTypeDto
        {
            Id = roomType.Id,
            Name = roomType.Name,
            Description = roomType.Description,
            BaseRate = roomType.BaseRate.Rate,
            NumberOfBeds = roomType.Beds.NumberOfBeds,
            BedType = roomType.Beds.BedType.ToString(),
            MaxOccupancyAdults = roomType.OccupancyPolicy.MaxOccupancyAdults,
            MaxOccupancyChildren = roomType.OccupancyPolicy.MaxOccupancyChildren,
        };
    }
}