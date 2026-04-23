using BrisaPMS.Application.UseCases.HouseKeeping.Commands.ReportIncident;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands.ReportIncident;

public class ReportIncidentCommandValidatorTests
{
    private readonly ReportIncidentCommandValidator _validator;

    public ReportIncidentCommandValidatorTests()
    {
        _validator = new ReportIncidentCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = new ReportIncidentCommand
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
        var command = new ReportIncidentCommand
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
        var command = new ReportIncidentCommand
        {
            HouseKeepingTaskId = Guid.NewGuid(),
            IncidentDescription = "Broken lamp found during inspection"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
