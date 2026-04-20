using BrisaPMS.Application.UseCases.Bookings.Commands.ConfirmBooking;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Bookings.Commands.ConfirmBooking;

public class ConfirmBookingCommandValidatorTests
{
    private readonly ConfirmBookingCommandValidator _validator;

    public ConfirmBookingCommandValidatorTests()
    {
        _validator = new ConfirmBookingCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenBookingIdIsEmpty()
    {
        // Arrange
        var command = CreateCommand(Guid.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BookingId);
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

    private static ConfirmBookingCommand CreateValidCommand()
    {
        return CreateCommand(Guid.NewGuid());
    }

    private static ConfirmBookingCommand CreateCommand(Guid bookingId)
    {
        return new ConfirmBookingCommand
        {
            BookingId = bookingId
        };
    }
}