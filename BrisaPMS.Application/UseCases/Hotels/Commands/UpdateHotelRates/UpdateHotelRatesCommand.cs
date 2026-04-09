using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Billing;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelRates;

public class UpdateHotelRatesCommand : IRequest<bool>
{
    public required Guid HotelId { get; set; }
    public required decimal ItbisRate { get; set; }
    public required decimal ServiceChargeRate { get; set; }
}