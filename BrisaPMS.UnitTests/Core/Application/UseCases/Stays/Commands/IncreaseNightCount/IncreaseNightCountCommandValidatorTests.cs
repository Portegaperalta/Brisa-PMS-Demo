using BrisaPMS.Application.UseCases.Stays.Commands.IncreaseNightCount;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Stays.Commands.IncreaseNightCount;

public class IncreaseNightCountCommandValidatorTests
{
    private readonly IncreaseNightCountCommandValidator _validator;

    public IncreaseNightCountCommandValidatorTests()
    {
        _validator = new IncreaseNightCountCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = new IncreaseNightCountCommand { StayId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StayId);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new IncreaseNightCountCommand { StayId = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
