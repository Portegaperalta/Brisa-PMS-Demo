using FluentValidation;

namespace BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeBaseRate;

public class UpdateRoomTypeBaseRateCommandValidator : AbstractValidator<UpdateRoomTypeBaseRateCommand>
{
    public UpdateRoomTypeBaseRateCommandValidator()
    {
        RuleFor(x => x.RoomTypeId)
            .NotEmpty().WithMessage("The field RoomTypeId is required.");
        
        RuleFor(x => x.NewBaseRate)
            .NotEmpty().WithMessage("The field Base Rate is required.")
            .GreaterThanOrEqualTo(0).WithMessage("Base rate can't be negative.")
            .LessThanOrEqualTo(100).WithMessage("Base rate can't be greater than 100%");
    }
}