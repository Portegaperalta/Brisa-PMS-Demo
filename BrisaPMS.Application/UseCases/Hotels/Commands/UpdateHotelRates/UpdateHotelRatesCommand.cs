using BrisaPMS.Domain.Billing;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelRates;

public class UpdateHotelRatesCommand
{
    public required Guid HotelId { get; set; }
    public required decimal ItbisRate { get; set; }
    public required decimal ServiceChargeRate { get; set; }
}