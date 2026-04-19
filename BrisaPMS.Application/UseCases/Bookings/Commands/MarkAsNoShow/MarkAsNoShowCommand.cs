using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.MarkAsNoShow;

public class MarkAsNoShowCommand : IRequest<bool>
{
    public required Guid BookingId { get; set; }
}