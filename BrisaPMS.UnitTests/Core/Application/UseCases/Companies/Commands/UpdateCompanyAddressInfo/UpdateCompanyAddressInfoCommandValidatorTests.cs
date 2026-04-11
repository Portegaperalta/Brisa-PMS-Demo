using BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyAddressInfo;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Companies.Commands.UpdateCompanyAddressInfo;

public class UpdateCompanyAddressInfoCommandValidatorTests
{
    private readonly UpdateCompanyAddressInfoCommandValidator _commandValidator;

    public UpdateCompanyAddressInfoCommandValidatorTests()
    {
        _commandValidator = new UpdateCompanyAddressInfoCommandValidator();
    }
    
    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = CreateUpdateCompanyAddressInfoCommand(Guid.Empty, "", null, "", "", "");
        
        // Act
        var result = _commandValidator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CompanyId);
        result.ShouldHaveValidationErrorFor(x => x.NewAddress1);
        result.ShouldHaveValidationErrorFor(x => x.NewCity);
        result.ShouldHaveValidationErrorFor(x => x.NewProvince);
        result.ShouldHaveValidationErrorFor(x => x.NewZipCode);
    }

    [Fact]
    public void Validator_HasErrors_WhenFieldsExceedsMaxLength()
    {
        // Arrange
        var command = CreateUpdateCompanyAddressInfoCommand
        (
            Guid.NewGuid(),
            new string('A', 201),
            new string('A', 201),
            new string('C', 101),
            new string('P', 101),
            new string('Z', 11)
        );
        
        // Act
        var result = _commandValidator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NewAddress1);
        result.ShouldHaveValidationErrorFor(x => x.NewAddress2);
        result.ShouldHaveValidationErrorFor(x => x.NewCity);
        result.ShouldHaveValidationErrorFor(x => x.NewProvince);
        result.ShouldHaveValidationErrorFor(x => x.NewZipCode);
    }

    [Theory]
    [InlineData("A12345")]
    [InlineData("@1B23456")]
    public void Validator_HasError_WhenZipCodeIsInvalid(string invalidZipCode)
    {
        // Arrange
        var command = CreateUpdateCompanyAddressInfoCommand
            (
                Guid.NewGuid(),
                CreateAddress1(),
                CreateAddress2(),
                CreateCity(),
                CreateProvince(),
                invalidZipCode
            );
        
        // Act
        var result = _commandValidator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NewZipCode);
    }
    
    // Helper methods
    private static UpdateCompanyAddressInfoCommand
        CreateUpdateCompanyAddressInfoCommand(Guid companyId, string newAddress1, string? newAddress2, string newCity,
            string newProvince, string newZipCode)
    {
        return new UpdateCompanyAddressInfoCommand
        {
            CompanyId = companyId,
            NewAddress1 = newAddress1,
            NewAddress2 = newAddress2,
            NewCity = newCity,
            NewProvince = newProvince,
            NewZipCode = newZipCode
        };
    }
    
    private static string CreateAddress1() => "223 North street";
    private static string CreateAddress2() => "Suite 2C";
    private static string CreateCity() => "Santiago";
    private static string CreateProvince() => "Cibao";
}
