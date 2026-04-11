using BrisaPMS.Application.UseCases.Rooms.Commands.SetAsPendingRestock;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Rooms.Commands.SetAsPendingRestock;

public class SetAsPendingRestockCommandValidatorTests
{
  private readonly SetAsPendingRestockCommandValidator _commandValidator;

  public SetAsPendingRestockCommandValidatorTests()
  {
    _commandValidator = new SetAsPendingRestockCommandValidator();
  }

  [Fact]
  public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
  {
    // Arrange
    var command = CreateSetAsPendingRestockCommand(Guid.Empty);

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.RoomId);
  }

  [Fact]
  public void Validator_HasNoErrors_WhenCommandIsValid()
  {
    // Arrange
    var command = CreateValidCommand();

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }

  private static SetAsPendingRestockCommand CreateValidCommand()
  {
    return CreateSetAsPendingRestockCommand(Guid.NewGuid());
  }

  private static SetAsPendingRestockCommand CreateSetAsPendingRestockCommand(Guid roomId)
  {
    return new SetAsPendingRestockCommand
    {
      RoomId = roomId
    };
  }
}
