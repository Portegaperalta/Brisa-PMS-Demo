using BrisaPMS.Application.UseCases.Rooms.Commands.UpdateRoomNumber;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Rooms.Commands.UpdateRoomNumber;

public class UpdateRoomNumberCommandValidatorTests
{
  private readonly UpdateRoomNumberCommandValidator _commandValidator;

  public UpdateRoomNumberCommandValidatorTests()
  {
    _commandValidator = new UpdateRoomNumberCommandValidator();
  }

  [Fact]
  public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
  {
    // Arrange
    var command = CreateUpdateRoomNumberCommand(Guid.Empty, string.Empty);

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.RoomId);
    result.ShouldHaveValidationErrorFor(x => x.Number);
  }

  [Fact]
  public void Validator_HasError_WhenNumberExceedsMaxLength()
  {
    // Arrange
    var command = CreateUpdateRoomNumberCommand(Guid.NewGuid(), new string('R', 51));

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Number);
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

  private static UpdateRoomNumberCommand CreateValidCommand()
  {
    return CreateUpdateRoomNumberCommand(Guid.NewGuid(), "305");
  }

  private static UpdateRoomNumberCommand CreateUpdateRoomNumberCommand(Guid roomId, string number)
  {
    return new UpdateRoomNumberCommand
    {
      RoomId = roomId,
      Number = number
    };
  }
}
