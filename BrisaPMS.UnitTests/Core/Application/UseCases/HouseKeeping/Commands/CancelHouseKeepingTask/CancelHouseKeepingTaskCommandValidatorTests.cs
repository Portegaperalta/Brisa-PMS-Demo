using BrisaPMS.Application.UseCases.HouseKeeping.Commands.CancelHouseKeepingTask;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands.CancelHouseKeepingTask;

public class CancelHouseKeepingTaskCommandValidatorTests
{
    private readonly CancelHouseKeepingTaskCommandValidator _validator;

    public CancelHouseKeepingTaskCommandValidatorTests()
    {
        _validator = new CancelHouseKeepingTaskCommandValidator();
    }

    [Fact]
    public void Validator_HasError_WhenHouseKeepingTaskIdIsEmpty()
    {
        // Arrange
        var command = new CancelHouseKeepingTaskCommand
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
        var command = new CancelHouseKeepingTaskCommand
        {
            HouseKeepingTaskId = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
