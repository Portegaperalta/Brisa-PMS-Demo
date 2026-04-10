using BrisaPMS.Application.UseCases.Rooms.Commands.ChangeRoomType;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Rooms.Commands.ChangeRoomType;

public class ChangeRoomTypeCommandValidatorTests
{
  private readonly ChangeRoomTypeCommandValidator _commandValidator;

  public ChangeRoomTypeCommandValidatorTests()
  {
    _commandValidator = new ChangeRoomTypeCommandValidator();
  }

  [Fact]
  public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
  {
    // Arrange
    var command = CreateChangeRoomTypeCommand(Guid.Empty, Guid.Empty);

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.RoomId);
    result.ShouldHaveValidationErrorFor(x => x.RoomTypeId);
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

  private static ChangeRoomTypeCommand CreateValidCommand()
  {
    return CreateChangeRoomTypeCommand(Guid.NewGuid(), Guid.NewGuid());
  }

  private static ChangeRoomTypeCommand CreateChangeRoomTypeCommand(Guid roomId, Guid roomTypeId)
  {
    return new ChangeRoomTypeCommand
    {
      RoomId = roomId,
      RoomTypeId = roomTypeId
    };
  }
}
