using FluentValidation;

namespace BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeOccupancyPolicy;

public class UpdateRoomTypeOccupancyPolicyCommandValidator : AbstractValidator<UpdateRoomTypeOccupancyPolicyCommand>
{
    public UpdateRoomTypeOccupancyPolicyCommandValidator()
    {
        RuleFor(x => x.RoomTypeId)
            .NotEmpty().WithMessage("The field RoomTypeId is required");
        
        RuleFor(x => x.MaxOccupancyAdults)
            .NotEmpty().WithMessage("The field Max Occupancy Adults is required")
            .GreaterThanOrEqualTo(1).WithMessage("Room type must accept at least one adult")
            .LessThanOrEqualTo(16).WithMessage("Room type must accept 16 adults or less");
        
        RuleFor(x => x.MaxOccupancyChildren)
            .NotEmpty().WithMessage("The field Max Occupancy Children is required")
            .GreaterThanOrEqualTo(0).WithMessage("The field Max Occupancy Children must be greater than or equal to zero")
            .LessThanOrEqualTo(10).WithMessage("Room type must accept 10 children or less");
    }
}