using BrisaPMS.Application.UseCases.Users.Commands.Login;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Users.Commands.Login;

public class LoginCommandValidatorTests
{
  private readonly LoginCommandValidator _validator;

  public LoginCommandValidatorTests()
  {
    _validator = new LoginCommandValidator();
  }

  [Fact]
  public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
  {
    // Arrange
    var command = new LoginCommand
    {
      Email = string.Empty,
      Password = string.Empty
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Email);
    result.ShouldHaveValidationErrorFor(x => x.Password);
  }

  [Fact]
  public void Validator_HasErrors_WhenEmailExceedsMaxLength()
  {
    // Arrange
    var command = new LoginCommand
    {
      Email = new string('E', 255) + "@test.com",
      Password = "Test@1234"
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
    var command = new LoginCommand
    {
      Email = email,
      Password = "Test@1234"
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
    var command = new LoginCommand
    {
      Email = "test@example.com",
      Password = "Test@1234"
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }
}