using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.UpdateSpecialRequests;

public class UpdateSpecialRequestsCommand : IRequest<bool>
{
    public required Guid BookingId { get; set; }
    public required string SpecialRequests { get; set; }
}