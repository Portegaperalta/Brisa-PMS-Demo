using BrisaPMS.Domain.Shared.Enums;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelDefaultCurrency;

public class UpdateHotelDefaultCurrencyCommand
{
    public required Guid Id { get; set; }
    public required CurrencyCode CurrencyCode { get; set; }
}