using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.UseCases.Bookings.Queries.GetBookingsByHotelId;
using BrisaPMS.Application.UseCases.Bookings.Queries.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Bookings.Queries.GetAllBookings;

public class GetAllBookingsUseCase : IRequestHandler<GetAllBookingsQuery, List<BookingDto>>
{
    private readonly IBookingsRepository _bookingsRepository;
    
    public GetAllBookingsUseCase(IBookingsRepository bookingsRepository)
    {
        _bookingsRepository = bookingsRepository;
    }

    public async Task<List<BookingDto>> Handle(GetAllBookingsQuery query)
    {
        var bookings = await _bookingsRepository.GetAll();
        var bookingsDtos = bookings.Select(b => b.ToDto()).ToList();
        return bookingsDtos;
    }
}