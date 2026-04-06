using FluentValidation;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelRates;

public class UpdateHotelRatesValidator : AbstractValidator<UpdateHotelRatesCommand>
{
    public UpdateHotelRatesValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Field Hotel Id is required");
        
        RuleFor(x => x.ItbisRate)
            .NotEmpty().WithMessage("Field Itbis rate is required")
            .GreaterThan(0).WithMessage("Itbis rate can't be negative")
            .LessThan(100).WithMessage("Itbis rate can't be greater than 100");
        
        RuleFor(x => x.ServiceChargeRate)
            .NotEmpty().WithMessage("Field Service Charge rate is required")
            .GreaterThan(0).WithMessage("Service Charge rate can't be negative")
            .LessThan(100).WithMessage("Service Charge rate can't be greater than 100");
    }
}