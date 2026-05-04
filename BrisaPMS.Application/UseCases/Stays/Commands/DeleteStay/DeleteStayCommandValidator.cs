using FluentValidation;

namespace BrisaPMS.Application.UseCases.Stays.Commands.DeleteStay;

public class DeleteStayCommandValidator : AbstractValidator<DeleteStayCommand>
{
    public DeleteStayCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("The field StayId is required.");
    }
}