using BrisaPMS.Application.UseCases.Amenities.Commands.DeactivateAmenity;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Amenities.Commands.DeactivateAmenity;

public class DeactivateAmenityCommandValidatorTests
{
    private readonly DeactivateAmenityCommandValidator _validator;

    public DeactivateAmenityCommandValidatorTests()
    {
        _validator = new DeactivateAmenityCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = new DeactivateAmenityCommand { AmenityId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AmenityId);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new DeactivateAmenityCommand { AmenityId = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
