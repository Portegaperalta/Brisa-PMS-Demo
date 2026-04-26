using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Domain.Guests;
using Microsoft.EntityFrameworkCore;

namespace BrisaPMS.Persistence.Repositories;

public class GuestsRepository : Repository<Guest>, IGuestsRepository
{
    private readonly BrisaPmsDbContext _context;
    
    public GuestsRepository(BrisaPmsDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Guest>> GetAllByHotelIdAsync(Guid hotelId)
    {
        return await _context.Guests
            .Where(g => g.HotelId == hotelId)
            .ToListAsync();
    }
}