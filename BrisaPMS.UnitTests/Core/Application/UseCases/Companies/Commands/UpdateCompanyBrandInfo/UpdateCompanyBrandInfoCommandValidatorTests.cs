using BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyBrandInfo;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Companies.Commands.UpdateCompanyBrandInfo;

public class UpdateCompanyBrandInfoCommandValidatorTests
{
    private readonly UpdateCompanyBrandInfoCommandValidator _commandValidator;

    public UpdateCompanyBrandInfoCommandValidatorTests()
    {
        _commandValidator = new UpdateCompanyBrandInfoCommandValidator();
    }
    
    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = CreateUpdateCompanyBrandInfoCommand(Guid.Empty, "", "", null);
        
        // Act
        var result = _commandValidator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CompanyId);
        result.ShouldHaveValidationErrorFor(x => x.NewLegalName);
        result.ShouldHaveValidationErrorFor(x => x.NewCommercialName);
    }

    [Fact]
    public void Validator_HasErrors_WhenFieldsExceedsMaxLength()
    {
        // Arrange
        var command = CreateUpdateCompanyBrandInfoCommand
        (
            Guid.NewGuid(),
            new string('L', 251),
            new string('C', 251),
            new string('h', 2049)
        );
        
        // Act
        var result = _commandValidator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NewLegalName);
        result.ShouldHaveValidationErrorFor(x => x.NewCommercialName);
        result.ShouldHaveValidationErrorFor(x => x.NewLogoUrl);
    }

    [Theory]
    [InlineData("invalid-url")]
    [InlineData("ftp://example.com/logo.png")]
    [InlineData("htp://example.com/logo.png")]
    public void Validator_HasError_WhenLogoUrlIsInvalid(string invalidLogoUrl)
    {
        // Arrange
        var command = CreateUpdateCompanyBrandInfoCommand
            (
                Guid.NewGuid(),
                CreateLegalName(),
                CreateCommercialName(),
                invalidLogoUrl
            );
        
        // Act
        var result = _commandValidator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NewLogoUrl);
    }
    
    // Helper methods
    private static UpdateCompanyBrandInfoCommand
        CreateUpdateCompanyBrandInfoCommand(Guid companyId, string newLegalName, string newCommercialName,
            string? newLogoUrl)
    {
        return new UpdateCompanyBrandInfoCommand
        {
            CompanyId = companyId,
            NewLegalName = newLegalName,
            NewCommercialName = newCommercialName,
            NewLogoUrl = newLogoUrl
        };
    }
    
    private static string CreateLegalName() => "Brisa Hospitality SRL";
    private static string CreateCommercialName() => "Brisa PMS";
}
