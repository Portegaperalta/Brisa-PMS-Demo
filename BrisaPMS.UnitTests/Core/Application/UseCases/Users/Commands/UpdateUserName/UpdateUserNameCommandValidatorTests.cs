using BrisaPMS.Application.UseCases.Users.Commands.UpdateUserName;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Users.Commands.UpdateUserName;

public class UpdateUserNameCommandValidatorTests
{
    private readonly UpdateUserNameCommandValidator _validator;

    public UpdateUserNameCommandValidatorTests()
    {
        _validator = new UpdateUserNameCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = new UpdateUserNameCommand
        {
            UserId = Guid.Empty,
            FirstName = string.Empty,
            LastName = string.Empty
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Fact]
    public void Validator_HasErrors_WhenFieldsExceedMaxLength()
    {
        // Arrange
        var command = new UpdateUserNameCommand
        {
            UserId = Guid.NewGuid(),
            FirstName = new string('F', 251),
            LastName = new string('L', 251)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new UpdateUserNameCommand
        {
            UserId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}