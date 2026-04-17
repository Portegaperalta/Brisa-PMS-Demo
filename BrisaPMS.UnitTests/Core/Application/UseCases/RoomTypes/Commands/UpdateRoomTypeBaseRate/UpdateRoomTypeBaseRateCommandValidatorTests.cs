using BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeBaseRate;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeBaseRate;

public class UpdateRoomTypeBaseRateCommandValidatorTests
{
    private readonly UpdateRoomTypeBaseRateCommandValidator _validator;

    public UpdateRoomTypeBaseRateCommandValidatorTests()
    {
        _validator = new UpdateRoomTypeBaseRateCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = CreateCommand(Guid.Empty, default);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RoomTypeId);
        result.ShouldHaveValidationErrorFor(x => x.NewBaseRate);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void Validator_HasErrors_WhenBaseRateIsOutOfRange(decimal newBaseRate)
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), newBaseRate);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NewBaseRate);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), 25m);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    private static UpdateRoomTypeBaseRateCommand CreateCommand(Guid roomTypeId, decimal newBaseRate)
    {
        return new UpdateRoomTypeBaseRateCommand
        {
            RoomTypeId = roomTypeId,
            NewBaseRate = newBaseRate
        };
    }
}
