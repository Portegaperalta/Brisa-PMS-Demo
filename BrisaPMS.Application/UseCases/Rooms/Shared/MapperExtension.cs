using BrisaPMS.Domain.Rooms;

namespace BrisaPMS.Application.UseCases.Rooms.Shared;

public class MapperExtension
{
    public static RoomDto ToDto(Room room)
    {
        return new RoomDto
        {
            Id = room.Id,
            HotelId = room.HotelId,
            Number = room.Number,
            Floor = room.Floor,
            Type = room.RoomType.Name,
            TotalBeds = room.RoomType.TotalBeds,
            MaxOccupancyAdults = room.RoomType.MaxOccupancyAdults,
            MaxOccupancyChildren = room.RoomType.MaxOccupancyChildren,
            BaseRate = room.RoomType.BaseRate,
            AvailabilityStatus = room.AvailabilityStatus.ToString(),
            HygieneStatus =  room.HygieneStatus.ToString(),
            LastCleanedAt = room.LastCleanedAt,
            LastCleanedBy = room.LastCleanedBy,
            NeedsRestocking = room.NeedsRestocking,
        };
    }
}