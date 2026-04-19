using BrisaPMS.Application.UseCases.Bookings.Queries.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Bookings.Queries.GetBookingsByHotelId;

public class GetBookingsByHotelIdQuery : IRequest<List<BookingDto>>
{
    public required Guid HotelId { get; set; }
}