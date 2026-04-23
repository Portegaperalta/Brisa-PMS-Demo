using BrisaPMS.Application.UseCases.HouseKeeping.Commands.CreateHouseKeepingTask;
using BrisaPMS.Domain.HouseKeeping;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands.CreateHouseKeepingTask;

public class CreateHouseKeepingTaskCommandValidatorTests
{
    private readonly CreateHouseKeepingTaskCommandValidator _validator;

    public CreateHouseKeepingTaskCommandValidatorTests()
    {
        _validator = new CreateHouseKeepingTaskCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = new CreateHouseKeepingTaskCommand
        {
            RoomId = Guid.Empty,
            AssignedTo = Guid.Empty,
            AssignedBy = Guid.Empty,
            HouseKeepingTaskType = string.Empty,
            TaskPriority = string.Empty,
            ExpectedStartTime = default,
            ExpectedEndTime = default,
            Notes = null
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RoomId);
        result.ShouldHaveValidationErrorFor(x => x.AssignedTo);
        result.ShouldHaveValidationErrorFor(x => x.AssignedBy);
        result.ShouldHaveValidationErrorFor(x => x.HouseKeepingTaskType);
        result.ShouldHaveValidationErrorFor(x => x.TaskPriority);
        result.ShouldHaveValidationErrorFor(x => x.ExpectedStartTime);
        result.ShouldHaveValidationErrorFor(x => x.ExpectedEndTime);
    }

    [Theory]
    [InlineData("Invalid")]
    [InlineData("cleaning")]
    public void Validator_HasError_WhenHouseKeepingTaskTypeIsInvalid(string invalidHouseKeepingTaskType)
    {
        // Arrange
        var command = CreateValidCommand();
        command.HouseKeepingTaskType = invalidHouseKeepingTaskType;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.HouseKeepingTaskType);
    }

    [Theory]
    [InlineData("Invalid")]
    [InlineData("high")]
    public void Validator_HasError_WhenTaskPriorityIsInvalid(string invalidTaskPriority)
    {
        // Arrange
        var command = CreateValidCommand();
        command.TaskPriority = invalidTaskPriority;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TaskPriority);
    }

    [Fact]
    public void Validator_HasErrors_WhenExpectedStartTimeIsNotEarlierThanExpectedEndTime()
    {
        // Arrange
        var command = CreateValidCommand();
        command.ExpectedStartTime = new DateTime(2026, 4, 1, 11, 0, 0, DateTimeKind.Utc);
        command.ExpectedEndTime = new DateTime(2026, 4, 1, 10, 0, 0, DateTimeKind.Utc);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ExpectedStartTime);
        result.ShouldHaveValidationErrorFor(x => x.ExpectedEndTime);
    }

    [Fact]
    public void Validator_HasErrors_WhenExpectedStartTimeEqualsExpectedEndTime()
    {
        // Arrange
        var sameTime = new DateTime(2026, 4, 1, 10, 0, 0, DateTimeKind.Utc);
        var command = CreateValidCommand();
        command.ExpectedStartTime = sameTime;
        command.ExpectedEndTime = sameTime;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ExpectedStartTime);
        result.ShouldHaveValidationErrorFor(x => x.ExpectedEndTime);
    }

    [Fact]
    public void Validator_HasError_WhenNotesExceedMaxLength()
    {
        // Arrange
        var command = CreateValidCommand();
        command.Notes = new string('N', 501);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Notes);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = CreateValidCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    private static CreateHouseKeepingTaskCommand CreateValidCommand()
    {
        return new CreateHouseKeepingTaskCommand
        {
            RoomId = Guid.NewGuid(),
            AssignedTo = Guid.NewGuid(),
            AssignedBy = Guid.NewGuid(),
            HouseKeepingTaskType = nameof(HouseKeepingTaskType.Cleaning),
            TaskPriority = nameof(TaskPriority.High),
            ExpectedStartTime = new DateTime(2026, 4, 1, 10, 0, 0, DateTimeKind.Utc),
            ExpectedEndTime = new DateTime(2026, 4, 1, 11, 0, 0, DateTimeKind.Utc),
            Notes = "Clean room before next guest arrival"
        };
    }
}
