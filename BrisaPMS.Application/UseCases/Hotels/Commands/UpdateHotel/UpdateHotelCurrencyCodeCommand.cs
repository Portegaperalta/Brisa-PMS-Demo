using BrisaPMS.Domain.Shared.Enums;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotel;

public class UpdateHotelCurrencyCodeCommand
{
    public required Guid Id { get; set; }
    public required CurrencyCode CurrencyCode { get; set; }
}