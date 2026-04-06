using FluentValidation;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.ActivateHotel;

public class ActivateHotelValidator : AbstractValidator<ActivateHotelCommand>
{
    public ActivateHotelValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Field Hotel Id is required");
    }
}