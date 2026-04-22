using BrisaPMS.Application.UseCases.Users.Commands.ChangeRole;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Users.Commands.ChangeRole;

public class ChangeRoleCommandValidatorTests
{
  private readonly ChangeRoleCommandValidator _validator;

  public ChangeRoleCommandValidatorTests()
  {
    _validator = new ChangeRoleCommandValidator();
  }

  [Fact]
  public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
  {
    // Arrange
    var command = new ChangeRoleCommand
    {
      UserId = Guid.Empty,
      Role = string.Empty
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.UserId);
    result.ShouldHaveValidationErrorFor(x => x.Role);
  }

  [Fact]
  public void Validator_HasErrors_WhenRoleIsInvalid()
  {
    // Arrange
    var command = new ChangeRoleCommand
    {
      UserId = Guid.NewGuid(),
      Role = "InvalidRole"
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Role);
  }

  [Theory]
  [InlineData("Admin")]
  [InlineData("Manager")]
  [InlineData("Receptionist")]
  public void Validator_HasNoErrors_WhenRoleIsValid(string role)
  {
    // Arrange
    var command = new ChangeRoleCommand
    {
      UserId = Guid.NewGuid(),
      Role = role
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldNotHaveValidationErrorFor(x => x.Role);
  }

  [Fact]
  public void Validator_HasNoErrors_WhenCommandIsValid()
  {
    // Arrange
    var command = new ChangeRoleCommand
    {
      UserId = Guid.NewGuid(),
      Role = "Admin"
    };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }
}