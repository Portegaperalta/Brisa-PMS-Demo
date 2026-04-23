using BrisaPMS.Application.UseCases.HouseKeeping.Commands.CompleteHouseKeepingTask;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands.CompleteHouseKeepingTask;

public class CompleteHouseKeepingTaskCommandValidatorTests
{
    private readonly CompleteHouseKeepingTaskCommandValidator _validator;

    public CompleteHouseKeepingTaskCommandValidatorTests()
    {
        _validator = new CompleteHouseKeepingTaskCommandValidator();
    }

    [Fact]
    public void Validator_HasError_WhenHouseKeepingTaskIdIsEmpty()
    {
        // Arrange
        var command = new CompleteHouseKeepingTaskCommand
        {
            HouseKeepingTaskId = Guid.Empty
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.HouseKeepingTaskId);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new CompleteHouseKeepingTaskCommand
        {
            HouseKeepingTaskId = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
