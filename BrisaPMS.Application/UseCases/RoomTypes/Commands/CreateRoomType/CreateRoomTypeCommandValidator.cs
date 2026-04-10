using BrisaPMS.Domain.RoomTypes;
using FluentValidation;

namespace BrisaPMS.Application.UseCases.RoomTypes.Commands.CreateRoomType;

public class CreateRoomTypeCommandValidator : AbstractValidator<CreateRoomTypeCommand>
{
    public CreateRoomTypeCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("The field Name is required.")
            .MaximumLength(100).WithMessage("The field Name cannot exceed 100 characters.");
        
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("The field Description cannot exceed 500 characters.");
        
        RuleFor(x => x.BaseRate)
            .NotEmpty().WithMessage("The field BaseRate is required.")
            .GreaterThanOrEqualTo(0).WithMessage("The field BaseRate can't be negative.")
            .LessThanOrEqualTo(100).WithMessage("The field Base Rate can't be greater than 100");
        
        RuleFor(x => x.TotalBeds)
            .NotEmpty().WithMessage("The field Total Beds is required.")
            .GreaterThanOrEqualTo(1).WithMessage("The field Total Beds must be  greater than or equal to 1.")
            .LessThanOrEqualTo(100).WithMessage("The field Total Beds can't be greater than 100.");
        
        RuleFor(x => x.BedType)
            .NotEmpty().WithMessage("The field Bed Type is required.")
            .MaximumLength(20).WithMessage("The field Bed Type cannot exceed 20 characters.")
            .Must(x => Enum.IsDefined(typeof(BedType), x)).WithMessage("Bed type not supported.");
        
        RuleFor(x => x.MaxOccupancyAdults)
            .NotEmpty().WithMessage("The field Max Occupancy Adults is required.")
            .GreaterThanOrEqualTo(1).WithMessage("The field Max Occupancy Adults  must be greater than or equal to 1.")
            .LessThanOrEqualTo(16).WithMessage("The field Max Occupancy Adults can't be greater than 16.");
        
        RuleFor(x => x.MaxOccupancyChildren)
            .NotEmpty().WithMessage("The field Max Occupancy Children is required.")
            .GreaterThanOrEqualTo(0).WithMessage("the field Max  Occupancy Children can't be negative.")
            .LessThanOrEqualTo(10).WithMessage("the field Max Occupancy Children can't be greater than 10.");
    }
}