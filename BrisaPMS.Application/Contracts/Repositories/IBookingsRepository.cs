using BrisaPMS.Domain.Bookings;
using BrisaPMS.Domain.Guests;

namespace BrisaPMS.Application.Contracts.Repositories;

public interface IBookingsRepository : IRepository<Booking>
{
    Task<List<Booking>> GetAllByHotelIdAsync(Guid hotelId);
    Task<string> GetBookingStatusAsync(Guid bookingId);
}