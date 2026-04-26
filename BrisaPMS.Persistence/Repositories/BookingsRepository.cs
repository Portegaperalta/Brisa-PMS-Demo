using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Domain.Bookings;
using Microsoft.EntityFrameworkCore;

namespace BrisaPMS.Persistence.Repositories;

public class BookingsRepository : Repository<Booking>, IBookingsRepository
{
    private readonly BrisaPmsDbContext _context;
    
    public BookingsRepository(BrisaPmsDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<List<Booking>> GetAllByHotelIdAsync(Guid hotelId)
    {
        return await _context.Bookings
                     .Where(b => b.HotelId == hotelId)
                     .ToListAsync();
    }

    public async Task<string> GetBookingStatusAsync(Guid bookingId)
    {
        return await _context.Bookings
            .Where(b => b.Id == bookingId)
            .Select(b => b.Status.ToString())
            .FirstAsync();
    }
}