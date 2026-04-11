using FluentValidation;

namespace BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyContactInfo;

public class UpdateCompanyContactInfoCommandValidator : AbstractValidator<UpdateCompanyContactInfoCommand>
{
    public UpdateCompanyContactInfoCommandValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty().WithMessage("The field CompanyId is required.");

        RuleFor(x => x.NewBusinessEmail)
            .NotEmpty().WithMessage("The field Business Email is required.")
            .MaximumLength(254).WithMessage("The field Business Email can't exceed 254 characters.")
            .EmailAddress().WithMessage("Must be a valid email address.");
        
        RuleFor(x => x.NewBusinessPhoneNumber)
            .NotEmpty().WithMessage("The field Business Phone Number is required.")
            .MaximumLength(25).WithMessage("The field Business Phone Number can't exceed 25 characters.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Must be a valid phone number");
    }
}