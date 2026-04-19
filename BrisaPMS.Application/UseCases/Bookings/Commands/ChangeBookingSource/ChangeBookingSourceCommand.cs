using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.ChangeBookingSource;

public class ChangeBookingSourceCommand : IRequest<bool>
{
    public required Guid BookingId { get; set; }
    public required string Source { get; set; }
}