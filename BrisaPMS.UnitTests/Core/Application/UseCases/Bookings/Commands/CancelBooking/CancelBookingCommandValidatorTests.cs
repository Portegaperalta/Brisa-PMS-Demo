using BrisaPMS.Application.UseCases.Bookings.Commands.CancelBooking;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Bookings.Commands.CancelBooking;

public class CancelBookingCommandValidatorTests
{
    private readonly CancelBookingCommandValidator _validator;

    public CancelBookingCommandValidatorTests()
    {
        _validator = new CancelBookingCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = CreateCommand(Guid.Empty, string.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BookingId);
        result.ShouldHaveValidationErrorFor(x => x.CancellationReason);
    }

    [Fact]
    public void Validator_HasErrors_WhenCancellationReasonExceedsLimit()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), new string('A', 256));

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CancellationReason);
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

    private static CancelBookingCommand CreateValidCommand()
    {
        return CreateCommand(Guid.NewGuid(), "Guest requested cancellation");
    }

    private static CancelBookingCommand CreateCommand(Guid bookingId, string cancellationReason)
    {
        return new CancelBookingCommand
        {
            BookingId = bookingId,
            CancellationReason = cancellationReason
        };
    }
}