using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Domain.Stays;
using Microsoft.EntityFrameworkCore;

namespace BrisaPMS.Persistence.Repositories;

public class StaysRepository : Repository<Stay>, IStaysRepository
{
    public StaysRepository(BrisaPmsDbContext context) 
        : base(context)
    {}

    public async Task<IEnumerable<Stay>> GetAllByHotelIdAsync(Guid hotelId)
    {
        return await Context.Stays
            .Where(s => s.HotelId == hotelId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Stay>> GetAllByRoomIdAsync(Guid roomId)
    {
        return await Context.Stays
            .Where(s => s.RoomId == roomId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Stay>> GetAllByGuestIdAsync(Guid guestId)
    {
        return await Context.Stays
            .Where(s => s.GuestId == guestId)
            .ToListAsync();
    }
}