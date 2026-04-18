using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.CancelBooking;

public class CancelBookingCommand : IRequest<bool>
{
    public required Guid BookingId { get; set; }
    public required string CancellationReason { get; set; }
}