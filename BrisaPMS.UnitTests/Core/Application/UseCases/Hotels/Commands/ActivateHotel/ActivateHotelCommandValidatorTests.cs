using BrisaPMS.Application.UseCases.Hotels.Commands.ActivateHotel;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Hotels.Commands.ActivateHotel;

public class ActivateHotelCommandValidatorTests
{
    private readonly ActivateHotelCommandValidator _validator;

    public ActivateHotelCommandValidatorTests()
    {
        _validator = new ActivateHotelCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = CreateActivateHotelCommand(Guid.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.HotelId);
    }

    // Helper methods
    private static ActivateHotelCommand CreateActivateHotelCommand(Guid hotelId)
    {
        return new ActivateHotelCommand
        {
            HotelId = hotelId
        };
    }
}
