using BrisaPMS.Domain.Bookings;
using BrisaPMS.Domain.Guests;

namespace BrisaPMS.Application.Contracts.Repositories;

public interface IBookingsRepository : IRepository<Booking>
{
    Task<Booking?> GetByHotelIdAsync(Guid hotelId, Guid bookingId);
    Task<List<Booking>> GetAllByHotelIdAsync(Guid hotelId);
    Task<string> GetBookingStatusAsync(Guid bookingId);
}