using BrisaPMS.Application.UseCases.Rooms.Commands.CreateRoom;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Application.UseCases.Rooms.Commands.CreateRoom;

public class CreateRoomCommandValidatorTests
{
  private readonly CreateRoomCommandValidator _commandValidator;

  public CreateRoomCommandValidatorTests()
  {
    _commandValidator = new CreateRoomCommandValidator();
  }

  [Fact]
  public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
  {
    // Arrange
    var command = CreateCreateRoomCommand(
        Guid.Empty,
        Guid.Empty,
        string.Empty,
        default,
        string.Empty,
        string.Empty);

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.HotelId);
    result.ShouldHaveValidationErrorFor(x => x.RoomTypeId);
    result.ShouldHaveValidationErrorFor(x => x.Number);
    result.ShouldHaveValidationErrorFor(x => x.Floor);
    result.ShouldHaveValidationErrorFor(x => x.AvailabilityStatus);
    result.ShouldHaveValidationErrorFor(x => x.HygieneStatus);
  }

  [Fact]
  public void Validator_HasErrors_WhenFieldsExceedMaxLength()
  {
    // Arrange
    var command = CreateCreateRoomCommand(
        Guid.NewGuid(),
        Guid.NewGuid(),
        new string('R', 101),
        CreateFloor(),
        new string('A', 12),
        new string('H', 12));

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Number);
    result.ShouldHaveValidationErrorFor(x => x.AvailabilityStatus);
    result.ShouldHaveValidationErrorFor(x => x.HygieneStatus);
  }

  [Theory]
  [InlineData(-201)]
  [InlineData(201)]
  public void Validator_HasError_WhenFloorIsOutOfRange(int invalidFloor)
  {
    // Arrange
    var command = CreateValidCommand();
    command.Floor = invalidFloor;

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Floor);
  }

  [Theory]
  [InlineData("Invalid")]
  [InlineData("available")]
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

  [Theory]
  [InlineData("Invalid")]
  [InlineData("clean")]
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

  private static CreateRoomCommand CreateValidCommand()
  {
    return CreateCreateRoomCommand(
        Guid.NewGuid(),
        Guid.NewGuid(),
        CreateNumber(),
        CreateFloor(),
        CreateAvailabilityStatus(),
        CreateHygieneStatus());
  }

  private static CreateRoomCommand CreateCreateRoomCommand(
      Guid hotelId,
      Guid roomTypeId,
      string number,
      int floor,
      string availabilityStatus,
      string hygieneStatus)
  {
    return new CreateRoomCommand
    {
      HotelId = hotelId,
      RoomTypeId = roomTypeId,
      Number = number,
      Floor = floor,
      AvailabilityStatus = availabilityStatus,
      HygieneStatus = hygieneStatus
    };
  }

  private static string CreateNumber() => "101";
  private static int CreateFloor() => 1;
  private static string CreateAvailabilityStatus() => "Available";
  private static string CreateHygieneStatus() => "Clean";
}
