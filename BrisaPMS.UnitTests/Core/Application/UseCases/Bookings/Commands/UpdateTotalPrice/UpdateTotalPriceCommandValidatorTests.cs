using BrisaPMS.Application.UseCases.Bookings.Commands.UpdateTotalPrice;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Bookings.Commands.UpdateTotalPrice;

public class UpdateTotalPriceCommandValidatorTests
{
    private readonly UpdateTotalPriceCommandValidator _validator;

    public UpdateTotalPriceCommandValidatorTests()
    {
        _validator = new UpdateTotalPriceCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = CreateCommand(Guid.Empty, default);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BookingId);
        result.ShouldHaveValidationErrorFor(x => x.TotalPrice);
    }

    [Fact]
    public void Validator_HasErrors_WhenTotalPriceIsNegative()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), -50.00m);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TotalPrice);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = CreateValidCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    private static UpdateTotalPriceCommand CreateValidCommand()
    {
        return CreateCommand(Guid.NewGuid(), 250.75m);
    }

    private static UpdateTotalPriceCommand CreateCommand(Guid bookingId, decimal totalPrice)
    {
        return new UpdateTotalPriceCommand
        {
            BookingId = bookingId,
            TotalPrice = totalPrice
        };
    }
}