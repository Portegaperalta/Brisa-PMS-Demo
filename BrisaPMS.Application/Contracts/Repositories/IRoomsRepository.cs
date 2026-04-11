using BrisaPMS.Domain.Rooms;

namespace BrisaPMS.Application.Contracts.Repositories;

public interface IRoomsRepository : IRepository<Room>
{
    Task<List<Room>> GetAllByHotelId(Guid hotelId);
}