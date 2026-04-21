using BrisaPMS.Application.UseCases.Bookings.Commands.UpdateCheckInOutTimes;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Bookings.Commands.UpdateCheckInOutTimes;

public class UpdateCheckInOutTimesCommandValidatorTests
{
    private readonly UpdateCheckInOutTimesCommandValidator _validator;

    public UpdateCheckInOutTimesCommandValidatorTests()
    {
        _validator = new UpdateCheckInOutTimesCommandValidator();
    }


    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = CreateCommand(Guid.Empty, default, default);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BookingId);
        result.ShouldHaveValidationErrorFor(x => x.CheckInTime);
        result.ShouldHaveValidationErrorFor(x => x.CheckOutTime);
    }

    [Fact]
    public void Validator_HasErrors_WhenCheckInTimeIsLaterThanCheckOutTime()
    {
        // Arrange
        var command = CreateCommand(
            Guid.NewGuid(),
            new DateTime(2026, 4, 22, 15, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 4, 20, 11, 0, 0, DateTimeKind.Utc));

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CheckInTime);
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

    private static UpdateCheckInOutTimesCommand CreateValidCommand()
    {
        return CreateCommand(
            Guid.NewGuid(),
            new DateTime(2026, 4, 20, 15, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 4, 22, 11, 0, 0, DateTimeKind.Utc));
    }

    private static UpdateCheckInOutTimesCommand CreateCommand(Guid bookingId, DateTime checkInTime, DateTime checkOutTime)
    {
        return new UpdateCheckInOutTimesCommand
        {
            BookingId = bookingId,
            CheckInTime = checkInTime,
            CheckOutTime = checkOutTime
        };
    }
}