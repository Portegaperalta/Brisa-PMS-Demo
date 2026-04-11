using BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyContactInfo;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Companies.Commands.UpdateCompanyContactInfo;

public class UpdateCompanyContactInfoCommandValidatorTests
{
    private readonly UpdateCompanyContactInfoCommandValidator _validator;

    public UpdateCompanyContactInfoCommandValidatorTests()
    {
        _validator = new UpdateCompanyContactInfoCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        var command = CreateUpdateCompanyContactInfoCommand(Guid.Empty, string.Empty, string.Empty);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.CompanyId);
        result.ShouldHaveValidationErrorFor(x => x.NewBusinessEmail);
        result.ShouldHaveValidationErrorFor(x => x.NewBusinessPhoneNumber);
    }

    [Fact]
    public void Validator_HasErrors_WhenFieldsExceedMaxLength()
    {
        var command = CreateUpdateCompanyContactInfoCommand(
            Guid.NewGuid(),
            $"{new string('a', 255)}@test.com",
            new string('1', 26));

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.NewBusinessEmail);
        result.ShouldHaveValidationErrorFor(x => x.NewBusinessPhoneNumber);
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("test.com")]
    public void Validator_HasError_WhenNewBusinessEmailIsInvalid(string invalidBusinessEmail)
    {
        var command = CreateUpdateCompanyContactInfoCommand(
            Guid.NewGuid(),
            invalidBusinessEmail,
            CreateBusinessPhoneNumber());

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.NewBusinessEmail);
    }

    [Theory]
    [InlineData("invalid-phone")]
    [InlineData("a1235678910")]
    public void Validator_HasError_WhenNewBusinessPhoneNumberIsInvalid(string invalidBusinessPhoneNumber)
    {
        var command = CreateUpdateCompanyContactInfoCommand(
            Guid.NewGuid(),
            CreateBusinessEmail(),
            invalidBusinessPhoneNumber);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.NewBusinessPhoneNumber);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        var command = CreateUpdateCompanyContactInfoCommand(
            Guid.NewGuid(),
            CreateBusinessEmail(),
            CreateBusinessPhoneNumber());

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    private static UpdateCompanyContactInfoCommand CreateUpdateCompanyContactInfoCommand(
        Guid companyId,
        string businessEmail,
        string businessPhoneNumber)
    {
        return new UpdateCompanyContactInfoCommand
        {
            CompanyId = companyId,
            NewBusinessEmail = businessEmail,
            NewBusinessPhoneNumber = businessPhoneNumber
        };
    }

    private static string CreateBusinessEmail() => "company@brisa.com";

    private static string CreateBusinessPhoneNumber() => "+18295554321";
}