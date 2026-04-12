using FluentValidation;

namespace BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyRnc;

public class UpdateCompanyRncCommandValidator : AbstractValidator<UpdateCompanyRncCommand>
{
    public UpdateCompanyRncCommandValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty().WithMessage("The field  CompanyId is required.");
        
        RuleFor(x => x.NewRnc)
            .NotEmpty().WithMessage("The field  Rnc is required.")
            .MinimumLength(9).WithMessage("The field  Rnc must be minimum 9 characters long.")
            .MaximumLength(11).WithMessage("The field  Rnc can't exceed 11 characters.")
            .Matches(@"^[0-9]+(-[0-9]+)*$")
            .WithMessage("RNC must contain only numbers and dashes");
    }
}