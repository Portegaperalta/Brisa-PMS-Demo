using BrisaPMS.Domain.Shared.Enums;
using FluentValidation;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelDefaultCurrency;

public class UpdateHotelDefaultCurrencyValidator : AbstractValidator<UpdateHotelDefaultCurrencyCommand>
{
    UpdateHotelDefaultCurrencyValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Field Hotel Id is required");
        
        RuleFor(x => x.DefaultCurrencyCode)
            .NotEmpty().WithMessage("CurrencyCode is required")
            .MaximumLength(3).WithMessage("CurrencyCode cannot exceed 3 characters")
            .Must(x => Enum.IsDefined(typeof(CurrencyCode), x)).WithMessage("Currency code not supported");
    }
}