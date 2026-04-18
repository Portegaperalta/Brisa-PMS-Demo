using System.Data;
using BrisaPMS.Domain.Guest;
using FluentValidation;

namespace BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestDocumentation;

public class UpdateGuestDocumentationCommandValidator : AbstractValidator<UpdateGuestDocumentationCommand>
{
    public UpdateGuestDocumentationCommandValidator()
    {
        RuleFor(x => x.GuestId)
            .NotEmpty().WithMessage("The field GuestId is required.");
        
        RuleFor(x => x.DocumentType)
            .NotEmpty().WithMessage("The field DocumentType is required.")
            .MaximumLength(20).WithMessage("The field Document Type can't exceed 20 characters.")
            .Must(x => Enum.IsDefined(typeof(GuestDocumentType), x))
            .WithMessage("Document type not supported.");

        RuleFor(x => x.DocumentNumber)
            .NotEmpty().WithMessage("The field Document Number is required.")
            .MaximumLength(250).WithMessage("The field Document Number can't exceed 250 characters.");
    }
}