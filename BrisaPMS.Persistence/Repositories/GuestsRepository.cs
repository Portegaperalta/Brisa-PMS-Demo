using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Domain.Guests;
using Microsoft.EntityFrameworkCore;

namespace BrisaPMS.Persistence.Repositories;

public class GuestsRepository : Repository<Guest>, IGuestsRepository
{
    public GuestsRepository(BrisaPmsDbContext context) : base(context)
    {
    }

    public async Task<List<Guest>> GetAllByHotelIdAsync(Guid hotelId)
    {
        return await Context.Guests
            .Where(g => g.HotelId == hotelId)
            .ToListAsync();
    }
}