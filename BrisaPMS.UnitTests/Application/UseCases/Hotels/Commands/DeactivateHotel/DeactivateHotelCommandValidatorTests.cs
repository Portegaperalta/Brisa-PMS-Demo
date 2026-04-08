using BrisaPMS.Application.UseCases.Hotels.Commands.DeactivateHotel;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Application.UseCases.Hotels.Commands.DeactivateHotel;

public class DeactivateHotelCommandValidatorTests
{
    private readonly DeactivateHotelCommandValidator _validator;

    public DeactivateHotelCommandValidatorTests()
    {
        _validator = new DeactivateHotelCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = CreateDeactivateHotelCommand(Guid.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.HotelId);
    }

    // Helper methods
    private static DeactivateHotelCommand CreateDeactivateHotelCommand(Guid hotelId)
    {
        return new DeactivateHotelCommand
        {
            HotelId = hotelId
        };
    }
}
