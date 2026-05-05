using BrisaPMS.Application.UseCases.HouseKeeping.Commands.DeleteHouseKeepingTask;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands.DeleteHouseKeepingTask;

public class DeleteHouseKeepingTaskCommandValidatorTests
{
    private readonly DeleteHouseKeepingTaskCommandValidator _validator;

    public DeleteHouseKeepingTaskCommandValidatorTests()
    {
        _validator = new DeleteHouseKeepingTaskCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = new DeleteHouseKeepingTaskCommand { Id = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new DeleteHouseKeepingTaskCommand { Id = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}