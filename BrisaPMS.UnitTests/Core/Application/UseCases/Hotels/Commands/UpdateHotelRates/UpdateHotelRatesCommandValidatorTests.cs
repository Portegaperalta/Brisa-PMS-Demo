using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelRates;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Hotels.Commands.UpdateHotelRates;

public class UpdateHotelRatesCommandValidatorTests
{
    private readonly UpdateHotelRatesCommandValidator _validator;

    public UpdateHotelRatesCommandValidatorTests()
    {
        _validator = new UpdateHotelRatesCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = CreateUpdateHotelRatesCommand(Guid.Empty, default, default);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.HotelId);
        result.ShouldHaveValidationErrorFor(x => x.ItbisRate);
        result.ShouldHaveValidationErrorFor(x => x.ServiceChargeRate);
    }

    [Theory]
    [InlineData(-1, 10)]
    [InlineData(101, 10)]
    [InlineData(10, -1)]
    [InlineData(10, 101)]
    public void Validator_HasErrors_WhenRatesAreOutOfRange(decimal invalidItbisRate, decimal invalidServiceChargeRate)
    {
        // Arrange
        var command = CreateUpdateHotelRatesCommand(Guid.NewGuid(), invalidItbisRate, invalidServiceChargeRate);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        if (invalidItbisRate is < 0 or >= 100)
            result.ShouldHaveValidationErrorFor(x => x.ItbisRate);

        if (invalidServiceChargeRate is < 0 or >= 100)
            result.ShouldHaveValidationErrorFor(x => x.ServiceChargeRate);
    }

    // Helper methods
    private static UpdateHotelRatesCommand CreateUpdateHotelRatesCommand(
        Guid hotelId,
        decimal itbisRate,
        decimal serviceChargeRate)
    {
        return new UpdateHotelRatesCommand
        {
            HotelId = hotelId,
            ItbisRate = itbisRate,
            ServiceChargeRate = serviceChargeRate
        };
    }
}
