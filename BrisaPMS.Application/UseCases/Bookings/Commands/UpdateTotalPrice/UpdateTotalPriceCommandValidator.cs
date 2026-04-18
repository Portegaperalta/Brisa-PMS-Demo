using FluentValidation;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.UpdateTotalPrice;

public class UpdateTotalPriceCommandValidator : AbstractValidator<UpdateTotalPriceCommand>
{
    public UpdateTotalPriceCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("The field BookingId is required.");
        
        RuleFor(x => x.TotalPrice)
            .NotEmpty().WithMessage("The field Total Price is required.")
            .GreaterThanOrEqualTo(0).WithMessage("Total price can't be negative.");
    }
}