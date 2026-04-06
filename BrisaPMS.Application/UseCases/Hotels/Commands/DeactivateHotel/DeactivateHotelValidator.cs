using FluentValidation;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.DeactivateHotel;

public class DeactivateHotelValidator : AbstractValidator<DeactivateHotelCommand>
{
    public DeactivateHotelValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Field Hotel Id is required");
    }
}