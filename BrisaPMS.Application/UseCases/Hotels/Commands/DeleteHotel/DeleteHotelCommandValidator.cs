using FluentValidation;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.DeleteHotel;

public class DeleteHotelCommandValidator : AbstractValidator<DeleteHotelCommand>
{
    public DeleteHotelCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("The field HotelId is required.");
    }
}