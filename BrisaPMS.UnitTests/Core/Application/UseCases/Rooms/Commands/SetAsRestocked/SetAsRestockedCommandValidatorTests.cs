using BrisaPMS.Application.UseCases.Rooms.Commands.SetAsRestocked;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Rooms.Commands.SetAsRestocked;

public class SetAsRestockedCommandValidatorTests
{
  private readonly SetAsRestockedCommandValidator _commandValidator;

  public SetAsRestockedCommandValidatorTests()
  {
    _commandValidator = new SetAsRestockedCommandValidator();
  }

  [Fact]
  public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
  {
    // Arrange
    var command = CreateSetAsRestockedCommand(Guid.Empty);

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

  private static SetAsRestockedCommand CreateValidCommand()
  {
    return CreateSetAsRestockedCommand(Guid.NewGuid());
  }

  private static SetAsRestockedCommand CreateSetAsRestockedCommand(Guid roomId)
  {
    return new SetAsRestockedCommand
    {
      RoomId = roomId
    };
  }
}
