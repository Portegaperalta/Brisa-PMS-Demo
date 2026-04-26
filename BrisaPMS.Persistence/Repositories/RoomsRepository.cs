using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Domain.Rooms;
using Microsoft.EntityFrameworkCore;

namespace BrisaPMS.Persistence.Repositories;

public class RoomsRepository : Repository<Room>, IRoomsRepository
{
    public RoomsRepository(BrisaPmsDbContext context)
        : base(context)
    {
    }

    public async Task<List<Room>> GetAllByHotelId(Guid hotelId)
    {
        return await Context.Rooms
            .Where(r => r.HotelId == hotelId)
            .ToListAsync();
    }
}