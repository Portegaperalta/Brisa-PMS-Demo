using BrisaPMS.Application.UseCases.Bookings.Commands.MarkAsNoShow;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Bookings.Commands.MarkAsNoShow;

public class MarkAsNoShowCommandValidatorTests
{
    private readonly MarkAsNoShowCommandValidator _validator;

    public MarkAsNoShowCommandValidatorTests()
    {
        _validator = new MarkAsNoShowCommandValidator();
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

    private static MarkAsNoShowCommand CreateValidCommand()
    {
        return CreateCommand(Guid.NewGuid());
    }

    private static MarkAsNoShowCommand CreateCommand(Guid bookingId)
    {
        return new MarkAsNoShowCommand
        {
            BookingId = bookingId
        };
    }
}