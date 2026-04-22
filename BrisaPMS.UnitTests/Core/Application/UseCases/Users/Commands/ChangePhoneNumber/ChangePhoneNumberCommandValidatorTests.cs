using BrisaPMS.Application.UseCases.Users.Commands.ChangePhoneNumber;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Users.Commands.ChangePhoneNumber;

public class ChangePhoneNumberCommandValidatorTests
{
  private readonly ChangePhoneNumberCommandValidator _validator;

  public ChangePhoneNumberCommandValidatorTests()
  {
    _validator = new ChangePhoneNumberCommandValidator();
  }

  [Fact]
  public void Validator_HasErrors_WhenUserIdIsEmpty()
  {
    // Arrange
    var command = new ChangePhoneNumberCommand
    {
      UserId = Guid.Empty,
      PhoneNumber = "+18095551234"
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.UserId);
  }

  [Fact]
  public void Validator_HasErrors_WhenPhoneNumberIsEmpty()
  {
    // Arrange
    var command = new ChangePhoneNumberCommand
    {
      UserId = Guid.NewGuid(),
      PhoneNumber = string.Empty
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
  }

  [Fact]
  public void Validator_HasErrors_WhenPhoneNumberExceedsMaxLength()
  {
    // Arrange
    var command = new ChangePhoneNumberCommand
    {
      UserId = Guid.NewGuid(),
      PhoneNumber = new string('1', 26)
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
  }

  [Fact]
  public void Validator_HasErrors_WhenPhoneNumberIsInvalid()
  {
    // Arrange
    var command = new ChangePhoneNumberCommand
    {
      UserId = Guid.NewGuid(),
      PhoneNumber = "invalid"
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
  }

  [Fact]
  public void Validator_HasNoErrors_WhenCommandIsValid()
  {
    // Arrange
    var command = new ChangePhoneNumberCommand
    {
      UserId = Guid.NewGuid(),
      PhoneNumber = "+18095551234"
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }
}