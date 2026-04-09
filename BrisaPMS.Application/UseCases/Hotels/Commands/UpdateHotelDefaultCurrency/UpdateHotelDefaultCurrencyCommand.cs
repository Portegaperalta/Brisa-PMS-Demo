using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Shared.Enums;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelDefaultCurrency;

public class UpdateHotelDefaultCurrencyCommand : IRequest<bool>
{
    public required Guid HotelId { get; set; }
    public required string DefaultCurrencyCode { get; set; }
}