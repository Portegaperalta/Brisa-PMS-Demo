using BrisaPMS.Application.UseCases.HouseKeeping.Commands.ReassignHouseKeepingTask;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands.ReassignHouseKeepingTask;

public class ReassignHouseKeepingTaskCommandValidatorTests
{
    private readonly ReassignHouseKeepingTaskCommandValidator _validator;

    public ReassignHouseKeepingTaskCommandValidatorTests()
    {
        _validator = new ReassignHouseKeepingTaskCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = new ReassignHouseKeepingTaskCommand
        {
            HouseKeepingTaskId = Guid.Empty,
            AssignedTo = Guid.Empty
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.HouseKeepingTaskId);
        result.ShouldHaveValidationErrorFor(x => x.AssignedTo);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new ReassignHouseKeepingTaskCommand
        {
            HouseKeepingTaskId = Guid.NewGuid(),
            AssignedTo = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
