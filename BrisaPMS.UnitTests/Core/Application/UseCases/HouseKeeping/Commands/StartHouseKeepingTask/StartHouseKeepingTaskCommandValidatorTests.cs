using BrisaPMS.Application.UseCases.HouseKeeping.Commands.StartHouseKeepingTask;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands.StartHouseKeepingTask;

public class StartHouseKeepingTaskCommandValidatorTests
{
    private readonly StartHouseKeepingTaskCommandValidator _validator;

    public StartHouseKeepingTaskCommandValidatorTests()
    {
        _validator = new StartHouseKeepingTaskCommandValidator();
    }

    [Fact]
    public void Validator_HasError_WhenHouseKeepingTaskIdIsEmpty()
    {
        // Arrange
        var command = new StartHouseKeepingTaskCommand
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
        var command = new StartHouseKeepingTaskCommand
        {
            HouseKeepingTaskId = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
