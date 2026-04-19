using BrisaPMS.Application.UseCases.Bookings.Queries.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Bookings.Queries.GetAllBookings;

public class GetAllBookingsQuery : IRequest<List<BookingDto>>
{
}