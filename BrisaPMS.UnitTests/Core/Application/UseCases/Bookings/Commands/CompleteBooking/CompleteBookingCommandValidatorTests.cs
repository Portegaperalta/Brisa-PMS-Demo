using BrisaPMS.Application.UseCases.Bookings.Commands.CompleteBooking;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Bookings.Commands.CompleteBooking;

public class CompleteBookingCommandValidatorTests
{
    private readonly CompleteBookingCommandValidator _validator;

    public CompleteBookingCommandValidatorTests()
    {
        _validator = new CompleteBookingCommandValidator();
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

    private static CompleteBookingCommand CreateValidCommand()
    {
        return CreateCommand(Guid.NewGuid());
    }

    private static CompleteBookingCommand CreateCommand(Guid bookingId)
    {
        return new CompleteBookingCommand
        {
            BookingId = bookingId
        };
    }
}