using BrisaPMS.Application.UseCases.HouseKeeping.Commands.UpdateIncidentDescription;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands.UpdateIncidentDescription;

public class UpdateIncidentDescriptionCommandValidatorTests
{
    private readonly UpdateIncidentDescriptionCommandValidator _validator;

    public UpdateIncidentDescriptionCommandValidatorTests()
    {
        _validator = new UpdateIncidentDescriptionCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = new UpdateIncidentDescriptionCommand
        {
            HouseKeepingTaskId = Guid.Empty,
            IncidentDescription = string.Empty
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.HouseKeepingTaskId);
        result.ShouldHaveValidationErrorFor(x => x.IncidentDescription);
    }

    [Fact]
    public void Validator_HasError_WhenIncidentDescriptionExceedsMaxLength()
    {
        // Arrange
        var command = new UpdateIncidentDescriptionCommand
        {
            HouseKeepingTaskId = Guid.NewGuid(),
            IncidentDescription = new string('I', 501)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.IncidentDescription);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new UpdateIncidentDescriptionCommand
        {
            HouseKeepingTaskId = Guid.NewGuid(),
            IncidentDescription = "Updated incident details"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
