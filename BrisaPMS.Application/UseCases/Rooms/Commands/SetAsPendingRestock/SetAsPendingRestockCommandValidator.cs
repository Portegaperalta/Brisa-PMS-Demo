using FluentValidation;

namespace BrisaPMS.Application.UseCases.Rooms.Commands.SetAsPendingRestock;

public class SetAsPendingRestockCommandValidator : AbstractValidator<SetAsPendingRestockCommand>
{
    public SetAsPendingRestockCommandValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("Field RoomId is required");
    }
}