using BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeGeneralInfo;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeGeneralInfo;

public class UpdateRoomTypeGeneralInfoCommandValidatorTests
{
    private readonly UpdateRoomTypeGeneralInfoCommandValidator _validator;

    public UpdateRoomTypeGeneralInfoCommandValidatorTests()
    {
        _validator = new UpdateRoomTypeGeneralInfoCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = CreateCommand(Guid.Empty, string.Empty, "Ocean view room");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RoomTypeId);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validator_HasErrors_WhenFieldsExceedMaxLength()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), new string('R', 101), new string('D', 501));

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), "Deluxe Suite", "Ocean view room");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    private static UpdateRoomTypeGeneralInfoCommand CreateCommand(Guid roomTypeId, string name, string? description)
    {
        return new UpdateRoomTypeGeneralInfoCommand
        {
            RoomTypeId = roomTypeId,
            Name = name,
            Description = description
        };
    }
}
