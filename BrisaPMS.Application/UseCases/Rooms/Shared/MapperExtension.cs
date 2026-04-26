using BrisaPMS.Domain.Rooms;

namespace BrisaPMS.Application.UseCases.Rooms.Shared;

public static class MapperExtension
{
    public static RoomDto ToDto(this Room room)
    {
        return new RoomDto
        {
            Id = room.Id,
            RoomTypeId = room.RoomTypeId,
            HotelId = room.HotelId,
            Number = room.Number,
            Floor = room.Floor,
            AvailabilityStatus = room.AvailabilityStatus.ToString(),
            HygieneStatus =  room.HygieneStatus.ToString(),
            LastCleanedAt = room.LastCleanedAt,
            LastCleanedBy = room.LastCleanedBy,
            NeedsRestocking = room.NeedsRestocking,
        };
    }
}