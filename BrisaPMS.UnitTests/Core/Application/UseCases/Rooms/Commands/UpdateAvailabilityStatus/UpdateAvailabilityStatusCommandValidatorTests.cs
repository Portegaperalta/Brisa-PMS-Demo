using BrisaPMS.Application.UseCases.Rooms.Commands.UpdateAvailabilityStatus;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Rooms.Commands.UpdateAvailabilityStatus;

public class UpdateAvailabilityStatusCommandValidatorTests
{
  private readonly UpdateAvailabilityStatusCommandValidator _commandValidator;

  public UpdateAvailabilityStatusCommandValidatorTests()
  {
    _commandValidator = new UpdateAvailabilityStatusCommandValidator();
  }

  [Fact]
  public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
  {
    // Arrange
    var command = CreateUpdateAvailabilityStatusCommand(Guid.Empty, string.Empty);

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.RoomId);
    result.ShouldHaveValidationErrorFor(x => x.AvailabilityStatus);
  }

  [Fact]
  public void Validator_HasError_WhenAvailabilityStatusExceedsMaxLength()
  {
    // Arrange
    var command = CreateUpdateAvailabilityStatusCommand(Guid.NewGuid(), new string('A', 12));

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.AvailabilityStatus);
  }

  [Theory]
  [InlineData("Invalid")]
  [InlineData("Full")]
  public void Validator_HasError_WhenAvailabilityStatusIsInvalid(string invalidAvailabilityStatus)
  {
    // Arrange
    var command = CreateValidCommand();
    command.AvailabilityStatus = invalidAvailabilityStatus;

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.AvailabilityStatus);
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

  private static UpdateAvailabilityStatusCommand CreateValidCommand()
  {
    return CreateUpdateAvailabilityStatusCommand(Guid.NewGuid(), "Occupied");
  }

  private static UpdateAvailabilityStatusCommand CreateUpdateAvailabilityStatusCommand(Guid roomId, string availabilityStatus)
  {
    return new UpdateAvailabilityStatusCommand
    {
      RoomId = roomId,
      AvailabilityStatus = availabilityStatus
    };
  }
}
