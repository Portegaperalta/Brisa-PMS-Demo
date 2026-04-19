using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Bookings.Queries.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Bookings.Queries.GetBookingsByHotelId;

public class GetBookingsByHotelIdUseCase : IRequestHandler<GetBookingsByHotelIdQuery, List<BookingDto>>
{
    private readonly IBookingsRepository _bookingsRepository;
    private readonly IHotelsRepository _hotelsRepository;

    public GetBookingsByHotelIdUseCase(IBookingsRepository bookingsRepository, IHotelsRepository hotelsRepository)
    {
        _bookingsRepository = bookingsRepository;
        _hotelsRepository = hotelsRepository;
    }

    public async Task<List<BookingDto>> Handle(GetBookingsByHotelIdQuery query)
    {
        var hotelExists = await _hotelsRepository.Exists(query.HotelId);

        if (hotelExists is not true)
            throw new NotFoundException("Hotel", query.HotelId);

        var bookings = await _bookingsRepository.GetAllByHotelIdAsync(query.HotelId);
        var bookingsDtos = bookings.Select(x => x.ToDto()).ToList();
        return bookingsDtos;
    }
}