using FluentValidation;

namespace BrisaPMS.Application.UseCases.Stays.Commands.CreateStay;

public class CreateStayCommandValidator : AbstractValidator<CreateStayCommand>
{
    public CreateStayCommandValidator()
    {
        RuleFor(x => x.GuestId)
            .NotEmpty().WithMessage("The field GuestId is required.");
        
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("The field BookingId is required.");
    }
}