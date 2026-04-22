using FluentValidation;

namespace BrisaPMS.Application.UseCases.Users.Commands.UpdateUserName;

public class UpdateUserNameCommandValidator : AbstractValidator<UpdateUserNameCommand>
{
    public UpdateUserNameCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("The field UserId is required.");
        
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("The field First Name is required.")
            .MaximumLength(250).WithMessage("The field First Name can't exceed 250 characters.");
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("The field Last Name is required.")
            .MaximumLength(250).WithMessage("The field Last Name can't exceed 250 characters.");
    }
}