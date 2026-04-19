using FluentValidation;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.UpdateSpecialRequests;

public class UpdateSpecialRequestsCommandValidator : AbstractValidator<UpdateSpecialRequestsCommand>
{
    public UpdateSpecialRequestsCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("The field BookingId is required.");
        
        RuleFor(x => x.SpecialRequests)
            .NotEmpty().WithMessage("The field Special Requests is required.")
            .MaximumLength(500).WithMessage("The field Special Requests can't exceed 500 characters.");
    }
}