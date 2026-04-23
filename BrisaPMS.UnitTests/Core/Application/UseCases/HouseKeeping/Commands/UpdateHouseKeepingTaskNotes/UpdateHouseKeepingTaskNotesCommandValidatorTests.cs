using BrisaPMS.Application.UseCases.HouseKeeping.Commands.UpdateHouseKeepingTaskNotes;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands.UpdateHouseKeepingTaskNotes;

public class UpdateHouseKeepingTaskNotesCommandValidatorTests
{
    private readonly UpdateHouseKeepingTaskNotesCommandValidator _validator;

    public UpdateHouseKeepingTaskNotesCommandValidatorTests()
    {
        _validator = new UpdateHouseKeepingTaskNotesCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = new UpdateHouseKeepingTaskNotesCommand
        {
            HouseKeepingTaskId = Guid.Empty,
            Notes = string.Empty
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.HouseKeepingTaskId);
        result.ShouldHaveValidationErrorFor(x => x.Notes);
    }

    [Fact]
    public void Validator_HasError_WhenNotesExceedMaxLength()
    {
        // Arrange
        var command = new UpdateHouseKeepingTaskNotesCommand
        {
            HouseKeepingTaskId = Guid.NewGuid(),
            Notes = new string('N', 501)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Notes);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new UpdateHouseKeepingTaskNotesCommand
        {
            HouseKeepingTaskId = Guid.NewGuid(),
            Notes = "Replace towels and amenities"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
