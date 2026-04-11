using FluentValidation;

namespace BrisaPMS.Application.UseCases.Rooms.Commands.UpdateRoomNumber;

public class UpdateRoomNumberCommandValidator : AbstractValidator<UpdateRoomNumberCommand>
{
    public UpdateRoomNumberCommandValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("The field RoomId is required");
        
        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("The field Room Number is required")
            .MaximumLength(50).WithMessage("The field Room Number can't exceed 50 characters");
    }
}