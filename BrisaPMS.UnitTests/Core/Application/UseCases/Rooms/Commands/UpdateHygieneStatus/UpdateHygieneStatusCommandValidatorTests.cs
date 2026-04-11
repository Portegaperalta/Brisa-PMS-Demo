using BrisaPMS.Application.UseCases.Rooms.Commands.UpdateHygieneStatus;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Rooms.Commands.UpdateHygieneStatus;

public class UpdateHygieneStatusCommandValidatorTests
{
  private readonly UpdateHygieneStatusCommandValidator _commandValidator;

  public UpdateHygieneStatusCommandValidatorTests()
  {
    _commandValidator = new UpdateHygieneStatusCommandValidator();
  }

  [Fact]
  public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
  {
    // Arrange
    var command = CreateUpdateHygieneStatusCommand(Guid.Empty, Guid.Empty, string.Empty);

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.RoomId);
    result.ShouldHaveValidationErrorFor(x => x.UserId);
    result.ShouldHaveValidationErrorFor(x => x.HygieneStatus);
  }

  [Fact]
  public void Validator_HasError_WhenHygieneStatusExceedsMaxLength()
  {
    // Arrange
    var command = CreateUpdateHygieneStatusCommand(Guid.NewGuid(), Guid.NewGuid(), new string('H', 12));

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.HygieneStatus);
  }

  [Theory]
  [InlineData("Invalid")]
  [InlineData("AlmostClean")]
  public void Validator_HasError_WhenHygieneStatusIsInvalid(string invalidHygieneStatus)
  {
    // Arrange
    var command = CreateValidCommand();
    command.HygieneStatus = invalidHygieneStatus;

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.HygieneStatus);
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

  private static UpdateHygieneStatusCommand CreateValidCommand()
  {
    return CreateUpdateHygieneStatusCommand(Guid.NewGuid(), Guid.NewGuid(), "Dirty");
  }

  private static UpdateHygieneStatusCommand CreateUpdateHygieneStatusCommand(Guid roomId, Guid userId, string hygieneStatus)
  {
    return new UpdateHygieneStatusCommand
    {
      RoomId = roomId,
      UserId = userId,
      HygieneStatus = hygieneStatus
    };
  }
}
