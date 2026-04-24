using FluentValidation;

namespace BrisaPMS.Application.UseCases.Stays.Commands.CompleteStay;

public class CompleteStayCommandValidator : AbstractValidator<CompleteStayCommand>
{
    public CompleteStayCommandValidator()
    {
        RuleFor(x => x.StayId)
            .NotEmpty().WithMessage("The field StayId is required.");
    }
}