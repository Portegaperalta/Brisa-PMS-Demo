using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.UpdateTotalPrice;

public class UpdateTotalPriceCommand : IRequest<bool>
{
    public required Guid BookingId { get; set; }
    public required decimal TotalPrice { get; set; }
}