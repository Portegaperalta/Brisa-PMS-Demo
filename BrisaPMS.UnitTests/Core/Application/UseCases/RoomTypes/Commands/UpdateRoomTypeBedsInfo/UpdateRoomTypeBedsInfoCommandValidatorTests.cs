using BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeBedsInfo;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeBedsInfo;

public class UpdateRoomTypeBedsInfoCommandValidatorTests
{
    private readonly UpdateRoomTypeBedsInfoCommandValidator _validator;

    public UpdateRoomTypeBedsInfoCommandValidatorTests()
    {
        _validator = new UpdateRoomTypeBedsInfoCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = CreateCommand(Guid.Empty, string.Empty, 2);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RoomTypeId);
        result.ShouldHaveValidationErrorFor(x => x.BedType);
    }

    [Fact]
    public void Validator_HasError_WhenBedTypeExceedsMaxLength()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), new string('B', 31), 2);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BedType);
    }

    [Fact]
    public void Validator_HasError_WhenBedTypeIsNotSupported()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), "Invalid", 2);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BedType);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), "Double", 2);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    private static UpdateRoomTypeBedsInfoCommand CreateCommand(Guid roomTypeId, string bedType, int numberOfBeds)
    {
        return new UpdateRoomTypeBedsInfoCommand
        {
            RoomTypeId = roomTypeId,
            BedType = bedType,
            NumberOfBeds = numberOfBeds
        };
    }
}
