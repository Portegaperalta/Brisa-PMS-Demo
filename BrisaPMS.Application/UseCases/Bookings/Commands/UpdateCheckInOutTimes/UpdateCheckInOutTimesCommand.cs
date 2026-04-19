using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.UpdateCheckInOutTimes;

public class UpdateCheckInOutTimesCommand : IRequest<bool>
{
    public required Guid BookingId { get; set; }
    public required DateTime CheckInTime { get; set; }
    public required DateTime CheckOutTime { get; set; }
}