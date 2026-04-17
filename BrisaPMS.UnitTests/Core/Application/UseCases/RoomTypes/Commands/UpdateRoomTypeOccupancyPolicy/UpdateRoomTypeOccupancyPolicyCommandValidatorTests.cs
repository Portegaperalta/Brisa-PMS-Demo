using BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeOccupancyPolicy;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeOccupancyPolicy;

public class UpdateRoomTypeOccupancyPolicyCommandValidatorTests
{
    private readonly UpdateRoomTypeOccupancyPolicyCommandValidator _validator;

    public UpdateRoomTypeOccupancyPolicyCommandValidatorTests()
    {
        _validator = new UpdateRoomTypeOccupancyPolicyCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = CreateCommand(Guid.Empty, default, default);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RoomTypeId);
        result.ShouldHaveValidationErrorFor(x => x.MaxOccupancyAdults);
        result.ShouldHaveValidationErrorFor(x => x.MaxOccupancyChildren);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(17, 1)]
    [InlineData(2, 11)]
    public void Validator_HasErrors_WhenOccupancyValuesAreOutOfRange(int maxOccupancyAdults, int maxOccupancyChildren)
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), maxOccupancyAdults, maxOccupancyChildren);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        if (maxOccupancyAdults is <= 0 or > 16)
            result.ShouldHaveValidationErrorFor(x => x.MaxOccupancyAdults);

        if (maxOccupancyChildren is <= 0 or > 10)
            result.ShouldHaveValidationErrorFor(x => x.MaxOccupancyChildren);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), 2, 1);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    private static UpdateRoomTypeOccupancyPolicyCommand CreateCommand(
        Guid roomTypeId,
        int maxOccupancyAdults,
        int maxOccupancyChildren)
    {
        return new UpdateRoomTypeOccupancyPolicyCommand
        {
            RoomTypeId = roomTypeId,
            MaxOccupancyAdults = maxOccupancyAdults,
            MaxOccupancyChildren = maxOccupancyChildren
        };
    }
}
