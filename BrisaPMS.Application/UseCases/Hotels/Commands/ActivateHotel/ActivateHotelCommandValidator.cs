using FluentValidation;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.ActivateHotel;

public class ActivateHotelCommandValidator : AbstractValidator<ActivateHotelCommand>
{
    public ActivateHotelCommandValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Field Hotel Id is required");
    }
}