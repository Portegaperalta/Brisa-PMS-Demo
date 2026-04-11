using BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyRnc;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Companies.Commands.UpdateCompanyRnc;

public class UpdateCompanyRncCommandValidatorTests
{
    private readonly UpdateCompanyRncCommandValidator _validator;

    public UpdateCompanyRncCommandValidatorTests()
    {
        _validator = new UpdateCompanyRncCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        var command = CreateUpdateCompanyRncCommand(Guid.Empty, string.Empty);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.CompanyId);
        result.ShouldHaveValidationErrorFor(x => x.NewRnc);
    }

    [Fact]
    public void Validator_HasErrors_WhenFieldsExceedMaxLength()
    {
        var command = CreateUpdateCompanyRncCommand(Guid.NewGuid(), new string('1', 12));

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.NewRnc);
    }

    [Fact]
    public void Validator_HasErrors_WhenNewRncIsBelowMinLength()
    {
        var command = CreateUpdateCompanyRncCommand(Guid.NewGuid(), new string('1', 8));

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.NewRnc);
    }

    [Theory]
    [InlineData("abc123456")]
    [InlineData("123-456-78")]
    [InlineData("12345abcde")]
    public void Validator_HasError_WhenNewRncIsInvalid(string invalidRnc)
    {
        var command = CreateUpdateCompanyRncCommand(Guid.NewGuid(), invalidRnc);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.NewRnc);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        var command = CreateUpdateCompanyRncCommand(Guid.NewGuid(), CreateValidRnc());

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    private static UpdateCompanyRncCommand CreateUpdateCompanyRncCommand(Guid companyId, string newRnc)
    {
        return new UpdateCompanyRncCommand
        {
            CompanyId = companyId,
            NewRnc = newRnc
        };
    }

    private static string CreateValidRnc() => "123456789";
}