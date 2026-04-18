using BrisaPMS.Application.UseCases.Amenities.Commands.UpdateAmenityDetails;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Amenities.Commands.UpdateAmenityDetails;

public class UpdateAmenityDetailsCommandValidatorTests
{
    private readonly UpdateAmenityDetailsCommandValidator _validator;

    public UpdateAmenityDetailsCommandValidatorTests()
    {
        _validator = new UpdateAmenityDetailsCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = CreateCommand(Guid.Empty, string.Empty, string.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AmenityId);
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Validator_HasErrors_WhenFieldsExceedMaxLength()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), new string('N', 101), new string('D', 501));

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), "Gym Access", "Access to the gym area");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    private static UpdateAmenityDetailsCommand CreateCommand(Guid amenityId, string name, string description)
    {
        return new UpdateAmenityDetailsCommand
        {
            AmenityId = amenityId,
            Name = name,
            Description = description
        };
    }
}
