using BrisaPMS.Application.UseCases.Amenities.Commands.ActivateAmenity;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Amenities.Commands.ActivateAmenity;

public class ActivateAmenityCommandValidatorTests
{
    private readonly ActivateAmenityCommandValidator _validator;

    public ActivateAmenityCommandValidatorTests()
    {
        _validator = new ActivateAmenityCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = new ActivateAmenityCommand { AmenityId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AmenityId);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new ActivateAmenityCommand { AmenityId = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
