using BrisaPMS.Application.UseCases.Users.Commands.ChangePassword;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Users.Commands.ChangePassword;

public class ChangePasswordCommandValidatorTests
{
  private readonly ChangePasswordCommandValidator _validator;

  public ChangePasswordCommandValidatorTests()
  {
    _validator = new ChangePasswordCommandValidator();
  }

  [Fact]
  public void Validator_HasErrors_WhenUserIdIsEmpty()
  {
    // Arrange
    var command = new ChangePasswordCommand
    {
      UserId = Guid.Empty,
      CurrentPassword = "Current@123",
      NewPassword = "Valid@123"
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.UserId);
  }

  [Fact]
  public void Validator_HasErrors_WhenCurrentPasswordIsEmpty()
  {
    // Arrange
    var command = new ChangePasswordCommand
    {
      UserId = Guid.NewGuid(),
      CurrentPassword = string.Empty,
      NewPassword = "Valid@123"
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.CurrentPassword);
  }

  [Fact]
  public void Validator_HasErrors_WhenNewPasswordIsEmpty()
  {
    // Arrange
    var command = new ChangePasswordCommand
    {
      UserId = Guid.NewGuid(),
      CurrentPassword = "Current@123",
      NewPassword = string.Empty
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.NewPassword);
  }

  [Fact]
  public void Validator_HasErrors_WhenNewPasswordExceedsMaxLength()
  {
    // Arrange
    var command = new ChangePasswordCommand
    {
      UserId = Guid.NewGuid(),
      CurrentPassword = "Current@123",
      NewPassword = new string('P', 513)
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.NewPassword);
  }

  [Fact]
  public void Validator_HasErrors_WhenNewPasswordIsTooShort()
  {
    // Arrange
    var command = new ChangePasswordCommand
    {
      UserId = Guid.NewGuid(),
      CurrentPassword = "Current@123",
      NewPassword = "Abc@12"
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.NewPassword);
  }

  [Fact]
  public void Validator_HasErrors_WhenNewPasswordLacksUppercase()
  {
    // Arrange
    var command = new ChangePasswordCommand
    {
      UserId = Guid.NewGuid(),
      CurrentPassword = "Current@123",
      NewPassword = "password@123"
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.NewPassword);
  }

  [Fact]
  public void Validator_HasErrors_WhenNewPasswordLacksNumber()
  {
    // Arrange
    var command = new ChangePasswordCommand
    {
      UserId = Guid.NewGuid(),
      CurrentPassword = "Current@123",
      NewPassword = "Password@"
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.NewPassword);
  }

  [Fact]
  public void Validator_HasErrors_WhenNewPasswordLacksSpecialCharacter()
  {
    // Arrange
    var command = new ChangePasswordCommand
    {
      UserId = Guid.NewGuid(),
      CurrentPassword = "Current@123",
      NewPassword = "Password123"
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.NewPassword);
  }

  [Fact]
  public void Validator_HasNoErrors_WhenCommandIsValid()
  {
    // Arrange
    var command = new ChangePasswordCommand
    {
      UserId = Guid.NewGuid(),
      CurrentPassword = "Current@123",
      NewPassword = "Valid@123"
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }
}