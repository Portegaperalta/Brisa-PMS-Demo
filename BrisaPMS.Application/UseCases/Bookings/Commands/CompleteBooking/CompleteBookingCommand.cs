using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.CompleteBooking;

public class CompleteBookingCommand : IRequest<bool>
{
    public required Guid BookingId { get; set; }
}