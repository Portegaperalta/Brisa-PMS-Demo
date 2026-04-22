using BrisaPMS.Application.UseCases.Users.Commands.ChangeEmail;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Users.Commands.ChangeEmail;

public class ChangeEmailCommandValidatorTests
{
    private readonly ChangeEmailCommandValidator _validator;

    public ChangeEmailCommandValidatorTests()
    {
        _validator = new ChangeEmailCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = new ChangeEmailCommand
        {
            UserId = Guid.Empty,
            Email = string.Empty
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validator_HasErrors_WhenEmailExceedsMaxLength()
    {
        // Arrange
        var command = new ChangeEmailCommand
        {
            UserId = Guid.NewGuid(),
            Email = new string('E', 255) + "@test.com"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("@test.com")]
    public void Validator_HasErrors_WhenEmailIsInvalid(string email)
    {
        // Arrange
        var command = new ChangeEmailCommand
        {
            UserId = Guid.NewGuid(),
            Email = email
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new ChangeEmailCommand
        {
            UserId = Guid.NewGuid(),
            Email = "test@example.com"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}