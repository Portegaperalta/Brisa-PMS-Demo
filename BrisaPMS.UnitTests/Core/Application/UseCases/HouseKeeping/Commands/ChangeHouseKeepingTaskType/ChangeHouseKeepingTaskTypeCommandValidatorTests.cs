using BrisaPMS.Application.UseCases.HouseKeeping.Commands.ChangeHouseKeepingTaskType;
using BrisaPMS.Domain.HouseKeeping;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands.ChangeHouseKeepingTaskType;

public class ChangeHouseKeepingTaskTypeCommandValidatorTests
{
    private readonly ChangeHouseKeepingTaskTypeCommandValidator _validator;

    public ChangeHouseKeepingTaskTypeCommandValidatorTests()
    {
        _validator = new ChangeHouseKeepingTaskTypeCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = new ChangeHouseKeepingTaskTypeCommand
        {
            HouseKeepingTaskId = Guid.Empty,
            HouseKeepingTaskType = string.Empty
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.HouseKeepingTaskId);
        result.ShouldHaveValidationErrorFor(x => x.HouseKeepingTaskType);
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

    private static ChangeHouseKeepingTaskTypeCommand CreateValidCommand()
    {
        return new ChangeHouseKeepingTaskTypeCommand
        {
            HouseKeepingTaskId = Guid.NewGuid(),
            HouseKeepingTaskType = nameof(HouseKeepingTaskType.Cleaning)
        };
    }
}
