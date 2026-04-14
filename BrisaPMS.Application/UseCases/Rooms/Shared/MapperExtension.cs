using BrisaPMS.Domain.Rooms;

namespace BrisaPMS.Application.UseCases.Rooms.Shared;

public static class MapperExtension
{
    public static RoomDto ToDto(this Room room)
    {
        return new RoomDto
        {
            Id = room.Id,
            HotelId = room.HotelId,
            Number = room.Number,
            Floor = room.Floor,
            Type = room.RoomType.Name,
            TotalBeds = room.RoomType.Beds.NumberOfBeds,
            BedType = room.RoomType.Beds.BedType.ToString(),
            MaxOccupancyAdults = room.RoomType.OccupancyPolicy.MaxOccupancyAdults,
            MaxOccupancyChildren = room.RoomType.OccupancyPolicy.MaxOccupancyChildren,
            BaseRate = room.RoomType.BaseRate.Rate,
            AvailabilityStatus = room.AvailabilityStatus.ToString(),
            HygieneStatus =  room.HygieneStatus.ToString(),
            LastCleanedAt = room.LastCleanedAt,
            LastCleanedBy = room.LastCleanedBy,
            NeedsRestocking = room.NeedsRestocking,
        };
    }
}