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
      Password = "Valid@123"
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.UserId);
  }

  [Fact]
  public void Validator_HasErrors_WhenPasswordIsEmpty()
  {
    // Arrange
    var command = new ChangePasswordCommand
    {
      UserId = Guid.NewGuid(),
      Password = string.Empty
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Password);
  }

  [Fact]
  public void Validator_HasErrors_WhenPasswordExceedsMaxLength()
  {
    // Arrange
    var command = new ChangePasswordCommand
    {
      UserId = Guid.NewGuid(),
      Password = new string('P', 513)
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Password);
  }

  [Fact]
  public void Validator_HasErrors_WhenPasswordIsTooShort()
  {
    // Arrange
    var command = new ChangePasswordCommand
    {
      UserId = Guid.NewGuid(),
      Password = "Abc@12"
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Password);
  }

  [Fact]
  public void Validator_HasErrors_WhenPasswordLacksUppercase()
  {
    // Arrange
    var command = new ChangePasswordCommand
    {
      UserId = Guid.NewGuid(),
      Password = "password@123"
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Password);
  }

  [Fact]
  public void Validator_HasErrors_WhenPasswordLacksNumber()
  {
    // Arrange
    var command = new ChangePasswordCommand
    {
      UserId = Guid.NewGuid(),
      Password = "Password@"
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Password);
  }

  [Fact]
  public void Validator_HasErrors_WhenPasswordLacksSpecialCharacter()
  {
    // Arrange
    var command = new ChangePasswordCommand
    {
      UserId = Guid.NewGuid(),
      Password = "Password123"
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Password);
  }

  [Fact]
  public void Validator_HasNoErrors_WhenCommandIsValid()
  {
    // Arrange
    var command = new ChangePasswordCommand
    {
      UserId = Guid.NewGuid(),
      Password = "Valid@123"
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }
}