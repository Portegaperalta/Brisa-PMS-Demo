using FluentValidation;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.DeactivateHotel;

public class DeactivateHotelCommandValidator : AbstractValidator<DeactivateHotelCommand>
{
    public DeactivateHotelCommandValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Field Hotel Id is required");
    }
}