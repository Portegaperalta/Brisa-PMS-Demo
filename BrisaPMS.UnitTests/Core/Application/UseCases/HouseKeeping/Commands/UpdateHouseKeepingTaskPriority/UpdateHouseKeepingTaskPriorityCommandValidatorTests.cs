using BrisaPMS.Application.UseCases.HouseKeeping.Commands.UpdateHouseKeepingTaskPriority;
using BrisaPMS.Domain.HouseKeeping;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands.UpdateHouseKeepingTaskPriority;

public class UpdateHouseKeepingTaskPriorityCommandValidatorTests
{
    private readonly UpdateHouseKeepingTaskPriorityCommandValidator _validator;

    public UpdateHouseKeepingTaskPriorityCommandValidatorTests()
    {
        _validator = new UpdateHouseKeepingTaskPriorityCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = new UpdateHouseKeepingTaskPriorityCommand
        {
            HouseKeepingTaskId = Guid.Empty,
            TaskPriority = string.Empty
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.HouseKeepingTaskId);
        result.ShouldHaveValidationErrorFor(x => x.TaskPriority);
    }

    [Theory]
    [InlineData("Invalid")]
    [InlineData("high")]
    public void Validator_HasError_WhenTaskPriorityIsInvalid(string invalidTaskPriority)
    {
        // Arrange
        var command = new UpdateHouseKeepingTaskPriorityCommand
        {
            HouseKeepingTaskId = Guid.NewGuid(),
            TaskPriority = invalidTaskPriority
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TaskPriority);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new UpdateHouseKeepingTaskPriorityCommand
        {
            HouseKeepingTaskId = Guid.NewGuid(),
            TaskPriority = nameof(TaskPriority.High)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
