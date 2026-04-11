using BrisaPMS.Domain.Rooms;
using FluentValidation;

namespace BrisaPMS.Application.UseCases.Rooms.Commands.UpdateHygieneStatus;

public class UpdateHygieneStatusCommandValidator : AbstractValidator<UpdateHygieneStatusCommand>
{
    public UpdateHygieneStatusCommandValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("The field RoomId is required");
        
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("The field UserId is required");
        
        RuleFor(x => x.HygieneStatus)
            .NotEmpty().WithMessage("The field Hygiene Status is required")
            .MaximumLength(11).WithMessage("The field Hygiene Status can't exceed 11 characters")
            .Must(x => Enum.IsDefined(typeof(RoomHygieneStatus), x))
            .WithMessage("Hygiene status not supported");
    }
}