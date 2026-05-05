using BrisaPMS.Application.UseCases.Rooms.Commands.DeleteRoom;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Rooms.Commands.DeleteRoom;

public class DeleteRoomCommandValidatorTests
{
    private readonly DeleteRoomCommandValidator _validator;

    public DeleteRoomCommandValidatorTests()
    {
        _validator = new DeleteRoomCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = new DeleteRoomCommand { Id = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new DeleteRoomCommand { Id = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
