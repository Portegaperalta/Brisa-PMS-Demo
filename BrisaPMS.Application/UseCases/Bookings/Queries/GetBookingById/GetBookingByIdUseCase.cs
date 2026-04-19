using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Bookings.Queries.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Bookings.Queries.GetBookingById;

public class GetBookingByIdUseCase : IRequestHandler<GetBookingByIdQuery, BookingDto>
{
    private readonly IBookingsRepository _bookingsRepository;
    
    public GetBookingByIdUseCase(IBookingsRepository bookingsRepository)
    {
        _bookingsRepository = bookingsRepository;
    }

    public async Task<BookingDto> Handle(GetBookingByIdQuery query)
    {
        var booking = await _bookingsRepository.GetById(query.BookingId);

        if (booking is null)
            throw new NotFoundException("Booking", query.BookingId);
        
        var bookingDto = booking.ToDto();
        return bookingDto;
    }
}