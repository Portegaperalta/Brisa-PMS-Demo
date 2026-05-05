using BrisaPMS.Application.UseCases.RoomTypes.Commands.DeleteRoomType;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.RoomTypes.Commands.DeleteRoomType;

public class DeleteRoomTypeCommandValidatorTests
{
    private readonly DeleteRoomTypeCommandValidator _validator;

    public DeleteRoomTypeCommandValidatorTests()
    {
        _validator = new DeleteRoomTypeCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = new DeleteRoomTypeCommand { Id = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new DeleteRoomTypeCommand { Id = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
