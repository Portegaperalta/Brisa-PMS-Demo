using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.UpdateBookingGuestCount;

public class UpdateBookingGuestCountCommand : IRequest<bool>
{
    public required Guid BookingId { get; set; }
    public required int NumberOfAdults { get; set; }
    public required int NumberOfChildren { get; set; }
}