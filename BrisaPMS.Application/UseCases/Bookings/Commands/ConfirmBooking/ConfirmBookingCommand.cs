using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.ConfirmBooking;

public class ConfirmBookingCommand : IRequest<bool>
{
    public required Guid BookingId { get; init; }
}