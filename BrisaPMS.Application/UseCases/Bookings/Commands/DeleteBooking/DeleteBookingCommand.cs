using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.DeleteBooking;

public class DeleteBookingCommand : IRequest<bool>
{
    public required Guid Id { get; set; }
}