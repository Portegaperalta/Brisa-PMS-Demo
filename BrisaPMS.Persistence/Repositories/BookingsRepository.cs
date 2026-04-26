using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Domain.Bookings;
using Microsoft.EntityFrameworkCore;

namespace BrisaPMS.Persistence.Repositories;

public class BookingsRepository : Repository<Booking>, IBookingsRepository
{
    public BookingsRepository(BrisaPmsDbContext context)
        : base(context)
    {
    }

    public async Task<List<Booking>> GetAllByHotelIdAsync(Guid hotelId)
    {
        return await Context.Bookings
                     .Where(b => b.HotelId == hotelId)
                     .ToListAsync();
    }

    public async Task<string> GetBookingStatusAsync(Guid bookingId)
    {
        return await Context.Bookings
            .Where(b => b.Id == bookingId)
            .Select(b => b.Status.ToString())
            .FirstAsync();
    }
}