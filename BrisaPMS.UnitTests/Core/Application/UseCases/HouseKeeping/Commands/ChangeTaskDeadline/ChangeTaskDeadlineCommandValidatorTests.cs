using BrisaPMS.Application.UseCases.HouseKeeping.Commands.ChangeTaskDeadline;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands.ChangeTaskDeadline;

public class ChangeTaskDeadlineCommandValidatorTests
{
    private readonly ChangeTaskDeadlineCommandValidator _validator;

    public ChangeTaskDeadlineCommandValidatorTests()
    {
        _validator = new ChangeTaskDeadlineCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = new ChangeTaskDeadlineCommand
        {
            HouseKeepingTaskId = Guid.Empty,
            ExpectedStartTime = default,
            ExpectedEndTime = default
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.HouseKeepingTaskId);
        result.ShouldHaveValidationErrorFor(x => x.ExpectedStartTime);
        result.ShouldHaveValidationErrorFor(x => x.ExpectedEndTime);
    }

    [Fact]
    public void Validator_HasErrors_WhenExpectedStartTimeIsNotEarlierThanExpectedEndTime()
    {
        // Arrange
        var start = new DateTime(2026, 4, 1, 11, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2026, 4, 1, 10, 0, 0, DateTimeKind.Utc);
        var command = CreateCommand(start, end);

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
        var command = CreateCommand(sameTime, sameTime);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ExpectedStartTime);
        result.ShouldHaveValidationErrorFor(x => x.ExpectedEndTime);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = CreateCommand(
            new DateTime(2026, 4, 1, 10, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 4, 1, 11, 0, 0, DateTimeKind.Utc));

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    private static ChangeTaskDeadlineCommand CreateCommand(DateTime expectedStartTime, DateTime expectedEndTime)
    {
        return new ChangeTaskDeadlineCommand
        {
            HouseKeepingTaskId = Guid.NewGuid(),
            ExpectedStartTime = expectedStartTime,
            ExpectedEndTime = expectedEndTime
        };
    }
}
