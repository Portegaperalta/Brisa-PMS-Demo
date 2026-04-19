using BrisaPMS.Application.UseCases.Bookings.Queries.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Bookings.Queries.GetBookingById;

public class GetBookingByIdQuery : IRequest<BookingDto>
{
    public required Guid BookingId { get; set; }
}