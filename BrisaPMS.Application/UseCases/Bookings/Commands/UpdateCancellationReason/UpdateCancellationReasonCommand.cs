using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.UpdateCancellationReason;

public class UpdateCancellationReasonCommand : IRequest<bool>
{
    public required Guid BookingId { get; set; }
    public required string CancellationReason { get; set; }
}