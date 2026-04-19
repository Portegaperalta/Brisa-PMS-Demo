using FluentValidation;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.MarkAsNoShow;

public class MarkAsNoShowCommandValidator : AbstractValidator<MarkAsNoShowCommand>
{
    public MarkAsNoShowCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("The field BookingId is required.");
    }
}