using BrisaPMS.Domain.Billing;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelRates;

public class UpdateHotelRatesCommand
{
    public required Guid Id { get; set; }
    public required ItbisRate ItbisRate { get; set; }
    public required ServiceChargeRate ServiceChargeRate { get; set; }
}