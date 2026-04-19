using BrisaPMS.Application.UseCases.Bookings.Commands.CreateBooking;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Bookings.Commands.CreateBooking;

public class CreateBookingCommandValidatorTests
{
    private readonly CreateBookingCommandValidator _validator;

    public CreateBookingCommandValidatorTests()
    {
        _validator = new CreateBookingCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = CreateCommand(
            Guid.Empty,
            Guid.Empty,
            Guid.Empty,
            string.Empty,
            default,
            default,
            default,
            default,
            null,
            default,
            null);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.HotelId);
        result.ShouldHaveValidationErrorFor(x => x.RoomId);
        result.ShouldHaveValidationErrorFor(x => x.GuestId);
        result.ShouldHaveValidationErrorFor(x => x.Source);
        result.ShouldHaveValidationErrorFor(x => x.NumberOfAdults);
        result.ShouldHaveValidationErrorFor(x => x.NumberOfChildren);
        result.ShouldHaveValidationErrorFor(x => x.CheckInTime);
        result.ShouldHaveValidationErrorFor(x => x.CheckOutTime);
        result.ShouldHaveValidationErrorFor(x => x.TotalPrice);
    }

    [Fact]
    public void Validator_HasErrors_WhenFieldsExceedLimits()
    {
        // Arrange
        var command = CreateCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new string('S', 201),
            11,
            11,
            new DateTime(2026, 4, 20, 15, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 4, 22, 11, 0, 0, DateTimeKind.Utc),
            new string('R', 501),
            250.75m,
            Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Source);
        result.ShouldHaveValidationErrorFor(x => x.NumberOfAdults);
        result.ShouldHaveValidationErrorFor(x => x.NumberOfChildren);
        result.ShouldHaveValidationErrorFor(x => x.SpecialRequests);
    }

    [Theory]
    [InlineData("Invalid", 2, 1, "2026-04-20T15:00:00Z", "2026-04-22T11:00:00Z")]
    [InlineData("Website", 0, 1, "2026-04-20T15:00:00Z", "2026-04-22T11:00:00Z")]
    [InlineData("Website", 2, -1, "2026-04-20T15:00:00Z", "2026-04-22T11:00:00Z")]
    [InlineData("Website", 2, 1, "2026-04-22T11:00:00Z", "2026-04-20T15:00:00Z")]
    public void Validator_HasErrors_WhenFieldsAreInvalid(
        string source,
        int numberOfAdults,
        int numberOfChildren,
        string checkInTime,
        string checkOutTime)
    {
        // Arrange
        var command = CreateCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            source,
            numberOfAdults,
            numberOfChildren,
            DateTime.Parse(checkInTime),
            DateTime.Parse(checkOutTime),
            "High floor please",
            250.75m,
            Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        if (source == "Invalid")
            result.ShouldHaveValidationErrorFor(x => x.Source);

        if (numberOfAdults == 0)
            result.ShouldHaveValidationErrorFor(x => x.NumberOfAdults);

        if (numberOfChildren < 0)
            result.ShouldHaveValidationErrorFor(x => x.NumberOfChildren);

        if (DateTime.Parse(checkInTime) >= DateTime.Parse(checkOutTime))
        {
            result.ShouldHaveValidationErrorFor(x => x.CheckInTime);
            result.ShouldHaveValidationErrorFor(x => x.CheckOutTime);
        }
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

    private static CreateBookingCommand CreateValidCommand()
    {
        return CreateCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Website",
            2,
            1,
            new DateTime(2026, 4, 20, 15, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 4, 22, 11, 0, 0, DateTimeKind.Utc),
            "High floor please",
            250.75m,
            Guid.NewGuid());
    }

    private static CreateBookingCommand CreateCommand(
        Guid hotelId,
        Guid roomId,
        Guid guestId,
        string source,
        int numberOfAdults,
        int numberOfChildren,
        DateTime checkInTime,
        DateTime checkOutTime,
        string? specialRequests,
        decimal totalPrice,
        Guid? discountId)
    {
        return new CreateBookingCommand
        {
            HotelId = hotelId,
            RoomId = roomId,
            GuestId = guestId,
            Source = source,
            NumberOfAdults = numberOfAdults,
            NumberOfChildren = numberOfChildren,
            CheckInTime = checkInTime,
            CheckOutTime = checkOutTime,
            SpecialRequests = specialRequests,
            TotalPrice = totalPrice,
            DiscountId = discountId
        };
    }
}
