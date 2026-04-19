using BrisaPMS.Domain.Booking;
using BrisaPMS.Domain.Bookings;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Bookings;

public class BookingTests
{
    [Fact]
    public void Constructor_ShouldCreateBooking_WhenValuesAreValid()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var guestId = Guid.NewGuid();
        var checkInOutTimes = CreateCheckInOutTimes();
        var discountId = Guid.NewGuid();
        var totalPrice = CreateTotalPrice();
        var guestCount = CreateGuestCount();

        // Act
        var result = new Booking(
            hotelId,
            roomId,
            guestId,
            BookingSource.InPerson,
            guestCount,
            checkInOutTimes,
            totalPrice,
            "Late arrival",
            discountId);

        // Assert
        result.Id.Should().NotBe(Guid.Empty);
        result.HotelId.Should().Be(hotelId);
        result.RoomId.Should().Be(roomId);
        result.GuestId.Should().Be(guestId);
        result.Source.Should().Be(BookingSource.InPerson);
        result.GuestCount.Should().Be(guestCount);
        result.CheckInOutTimes.Should().Be(checkInOutTimes);
        result.SpecialRequests.Should().Be("Late arrival");
        result.Status.Should().Be(BookingStatus.Pending);
        result.CancellationReason.Should().BeNull();
        result.TotalPrice.Should().Be(totalPrice);
        result.DiscountId.Should().Be(discountId);
    }

    [Fact]
    public void Constructor_ShouldCreateBooking_WhenOptionalValuesAreNotProvided()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var guestId = Guid.NewGuid();

        // Act
        var result = new Booking(
            hotelId,
            roomId,
            guestId,
            BookingSource.ThirdParty,
            new GuestCount(2, 0),
            CreateCheckInOutTimes(),
            new Money(180m, CurrencyCode.DOP));

        // Assert
        result.SpecialRequests.Should().BeNull();
        result.DiscountId.Should().BeNull();
        result.Status.Should().Be(BookingStatus.Pending);
    }

    [Fact]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenHotelIdIsEmpty()
    {
        // Act
        Action act = () => _ = new Booking(
            Guid.Empty,
            Guid.NewGuid(),
            Guid.NewGuid(),
            BookingSource.InPerson,
            CreateGuestCount(),
            CreateCheckInOutTimes(),
            CreateTotalPrice());

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenRoomIdIsEmpty()
    {
        // Act
        Action act = () => _ = new Booking(
            Guid.NewGuid(),
            Guid.Empty,
            Guid.NewGuid(),
            BookingSource.InPerson,
            CreateGuestCount(),
            CreateCheckInOutTimes(),
            CreateTotalPrice());

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenGuestIdIsEmpty()
    {
        // Act
        Action act = () => _ = new Booking(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.Empty,
            BookingSource.InPerson,
            CreateGuestCount(),
            CreateCheckInOutTimes(),
            CreateTotalPrice());

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Theory]
    [InlineData((BookingSource)0)]
    [InlineData((BookingSource)99)]
    public void Constructor_ShouldThrowBusinessRuleException_WhenSourceIsInvalid(BookingSource bookingSource)
    {
        // Act
        Action act = () => _ = new Booking(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            bookingSource,
            CreateGuestCount(),
            CreateCheckInOutTimes(),
            CreateTotalPrice());

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void ChangeAssignedRoom_ShouldUpdateRoomId_WhenValueIsValid()
    {
        // Arrange
        var booking = CreateBooking();
        var newRoomId = Guid.NewGuid();

        // Act
        booking.ChangeAssignedRoom(newRoomId);

        // Assert
        booking.RoomId.Should().Be(newRoomId);
    }

    [Fact]
    public void ChangeAssignedRoom_ShouldThrowEmptyRequiredFieldException_WhenRoomIdIsEmpty()
    {
        // Arrange
        var booking = CreateBooking();

        // Act
        Action act = () => booking.ChangeAssignedRoom(Guid.Empty);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void UpdateSource_ShouldUpdateSource_WhenValueIsValid()
    {
        // Arrange
        var booking = CreateBooking();

        // Act
        booking.UpdateSource(BookingSource.Phone);

        // Assert
        booking.Source.Should().Be(BookingSource.Phone);
    }

    [Theory]
    [InlineData((BookingSource)0)]
    [InlineData((BookingSource)99)]
    public void UpdateSource_ShouldThrowBusinessRuleException_WhenValueIsInvalid(BookingSource newBookingSource)
    {
        // Arrange
        var booking = CreateBooking();

        // Act
        Action act = () => booking.UpdateSource(newBookingSource);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void UpdateSource_ShouldThrowBusinessRuleException_WhenBookingIsCompleted()
    {
        // Arrange
        var booking = CreateBooking();
        booking.SetAsCompleted();

        // Act
        Action act = () => booking.UpdateSource(BookingSource.Phone);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void UpdateSource_ShouldThrowBusinessRuleException_WhenBookingIsCancelled()
    {
        // Arrange
        var booking = CreateBooking();
        booking.SetAsCancelled("Guest requested cancellation");

        // Act
        Action act = () => booking.UpdateSource(BookingSource.Phone);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void UpdateGuestCount_ShouldUpdateGuestCount_WhenValueIsValid()
    {
        // Arrange
        var booking = CreateBooking();
        var newGuestCount = new GuestCount(3, 2);

        // Act
        booking.UpdateGuestCount(newGuestCount);

        // Assert
        booking.GuestCount.Should().Be(newGuestCount);
    }

    [Fact]
    public void UpdateGuestCount_ShouldThrowBusinessRuleException_WhenBookingIsCompleted()
    {
        // Arrange
        var booking = CreateBooking();
        booking.SetAsCompleted();

        // Act
        Action act = () => booking.UpdateGuestCount(new GuestCount(3, 2));

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void UpdateGuestCount_ShouldThrowBusinessRuleException_WhenBookingIsCancelled()
    {
        // Arrange
        var booking = CreateBooking();
        booking.SetAsCancelled("Guest requested cancellation");

        // Act
        Action act = () => booking.UpdateGuestCount(new GuestCount(3, 2));

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void UpdateCheckInOutTimes_ShouldUpdateCheckInOutTimes_WhenValueIsValid()
    {
        // Arrange
        var booking = CreateBooking();
        var newCheckInOutTimes = new CheckInOutTimes(
            new DateTime(2026, 4, 3, 15, 0, 0),
            new DateTime(2026, 4, 5, 12, 0, 0));

        // Act
        booking.UpdateCheckInOutTimes(newCheckInOutTimes);

        // Assert
        booking.CheckInOutTimes.Should().Be(newCheckInOutTimes);
    }

    [Fact]
    public void UpdateCheckInOutTimes_ShouldThrowBusinessRuleException_WhenBookingIsCompleted()
    {
        // Arrange
        var booking = CreateBooking();
        booking.SetAsCompleted();
        var newCheckInOutTimes = new CheckInOutTimes(
            new DateTime(2026, 4, 3, 15, 0, 0),
            new DateTime(2026, 4, 5, 12, 0, 0));

        // Act
        Action act = () => booking.UpdateCheckInOutTimes(newCheckInOutTimes);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void UpdateCheckInOutTimes_ShouldThrowBusinessRuleException_WhenBookingIsCancelled()
    {
        // Arrange
        var booking = CreateBooking();
        booking.SetAsCancelled("Guest requested cancellation");
        var newCheckInOutTimes = new CheckInOutTimes(
            new DateTime(2026, 4, 3, 15, 0, 0),
            new DateTime(2026, 4, 5, 12, 0, 0));

        // Act
        Action act = () => booking.UpdateCheckInOutTimes(newCheckInOutTimes);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void UpdateSpecialRequests_ShouldUpdateSpecialRequests_WhenValueIsValid()
    {
        // Arrange
        var booking = CreateBooking();

        // Act
        booking.UpdateSpecialRequests("High floor room");

        // Assert
        booking.SpecialRequests.Should().Be("High floor room");
    }

    [Fact]
    public void UpdateSpecialRequests_ShouldThrowBusinessRuleException_WhenBookingIsCompleted()
    {
        // Arrange
        var booking = CreateBooking();
        booking.SetAsCompleted();

        // Act
        Action act = () => booking.UpdateSpecialRequests("High floor room");

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void UpdateSpecialRequests_ShouldThrowBusinessRuleException_WhenBookingIsCancelled()
    {
        // Arrange
        var booking = CreateBooking();
        booking.SetAsCancelled("Guest requested cancellation");

        // Act
        Action act = () => booking.UpdateSpecialRequests("High floor room");

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void SetAsConfirmed_ShouldSetStatusToConfirmed_WhenBookingIsPending()
    {
        // Arrange
        var booking = CreateBooking();

        // Act
        booking.SetAsConfirmed();

        // Assert
        booking.Status.Should().Be(BookingStatus.Confirmed);
    }

    [Fact]
    public void SetAsConfirmed_ShouldThrowBusinessRuleException_WhenBookingIsAlreadyConfirmed()
    {
        // Arrange
        var booking = CreateBooking();
        booking.SetAsConfirmed();

        // Act
        Action act = () => booking.SetAsConfirmed();

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void SetAsConfirmed_ShouldThrowBusinessRuleException_WhenBookingIsCancelled()
    {
        // Arrange
        var booking = CreateBooking();
        booking.SetAsCancelled("Guest requested cancellation");

        // Act
        Action act = () => booking.SetAsConfirmed();

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void SetAsConfirmed_ShouldThrowBusinessRuleException_WhenBookingIsCompleted()
    {
        // Arrange
        var booking = CreateBooking();
        booking.SetAsCompleted();

        // Act
        Action act = () => booking.SetAsConfirmed();

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void SetAsCompleted_ShouldSetStatusToComplete_WhenBookingIsPending()
    {
        // Arrange
        var booking = CreateBooking();

        // Act
        booking.SetAsCompleted();

        // Assert
        booking.Status.Should().Be(BookingStatus.Complete);
    }

    [Fact]
    public void SetAsCompleted_ShouldThrowBusinessRuleException_WhenBookingIsAlreadyCompleted()
    {
        // Arrange
        var booking = CreateBooking();
        booking.SetAsCompleted();

        // Act
        Action act = () => booking.SetAsCompleted();

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void SetAsCompleted_ShouldThrowBusinessRuleException_WhenBookingIsCancelled()
    {
        // Arrange
        var booking = CreateBooking();
        booking.SetAsCancelled("Guest requested cancellation");

        // Act
        Action act = () => booking.SetAsCompleted();

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void SetAsNoShow_ShouldSetStatusToNoShow_WhenBookingIsPending()
    {
        // Arrange
        var booking = CreateBooking();

        // Act
        booking.SetAsNoShow();

        // Assert
        booking.Status.Should().Be(BookingStatus.NoShow);
    }

    [Fact]
    public void SetAsNoShow_ShouldThrowBusinessRuleException_WhenBookingIsAlreadyNoShow()
    {
        // Arrange
        var booking = CreateBooking();
        booking.SetAsNoShow();

        // Act
        Action act = () => booking.SetAsNoShow();

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void SetAsNoShow_ShouldThrowBusinessRuleException_WhenBookingIsCancelled()
    {
        // Arrange
        var booking = CreateBooking();
        booking.SetAsCancelled("Guest requested cancellation");

        // Act
        Action act = () => booking.SetAsNoShow();

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void SetAsNoShow_ShouldThrowBusinessRuleException_WhenBookingIsCompleted()
    {
        // Arrange
        var booking = CreateBooking();
        booking.SetAsCompleted();

        // Act
        Action act = () => booking.SetAsNoShow();

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void SetAsCancelled_ShouldSetStatusAndCancellationReason_WhenValueIsValid()
    {
        // Arrange
        var booking = CreateBooking();

        // Act
        booking.SetAsCancelled("Guest requested cancellation");

        // Assert
        booking.Status.Should().Be(BookingStatus.Cancelled);
        booking.CancellationReason.Should().Be("Guest requested cancellation");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void SetAsCancelled_ShouldThrowEmptyRequiredFieldException_WhenReasonIsNullOrWhiteSpace(string? cancellationReason)
    {
        // Arrange
        var booking = CreateBooking();

        // Act
        Action act = () => booking.SetAsCancelled(cancellationReason!);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void SetAsCancelled_ShouldThrowBusinessRuleException_WhenBookingIsAlreadyCancelled()
    {
        // Arrange
        var booking = CreateBooking();
        booking.SetAsCancelled("Guest requested cancellation");

        // Act
        Action act = () => booking.SetAsCancelled("Another reason");

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void SetAsCancelled_ShouldThrowBusinessRuleException_WhenBookingIsCompleted()
    {
        // Arrange
        var booking = CreateBooking();
        booking.SetAsCompleted();

        // Act
        Action act = () => booking.SetAsCancelled("Guest requested cancellation");

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void UpdateCancellationReason_ShouldUpdateCancellationReason_WhenBookingIsCancelled()
    {
        // Arrange
        var booking = CreateBooking();
        booking.SetAsCancelled("Guest requested cancellation");

        // Act
        booking.UpdateCancellationReason("Updated cancellation reason");

        // Assert
        booking.CancellationReason.Should().Be("Updated cancellation reason");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    public void UpdateCancellationReason_ShouldThrowEmptyRequiredFieldException_WhenValueIsNullOrWhiteSpace(string? newCancellationReason)
    {
        // Arrange
        var booking = CreateBooking();
        booking.SetAsCancelled("Guest requested cancellation");

        // Act
        Action act = () => booking.UpdateCancellationReason(newCancellationReason!);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void UpdateCancellationReason_ShouldThrowBusinessRuleException_WhenBookingIsNotCancelled()
    {
        // Arrange
        var booking = CreateBooking();

        // Act
        Action act = () => booking.UpdateCancellationReason("Updated cancellation reason");

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void UpdateTotalPrice_ShouldUpdateTotalPrice_WhenValueIsValid()
    {
        // Arrange
        var booking = CreateBooking();
        var newTotalPrice = new Money(320m, CurrencyCode.USD);

        // Act
        booking.UpdateTotalPrice(newTotalPrice);

        // Assert
        booking.TotalPrice.Should().Be(newTotalPrice);
    }

    [Fact]
    public void UpdateTotalPrice_ShouldThrowBusinessRuleException_WhenBookingIsCompleted()
    {
        // Arrange
        var booking = CreateBooking();
        booking.SetAsCompleted();
        var newTotalPrice = new Money(320m, CurrencyCode.USD);

        // Act
        Action act = () => booking.UpdateTotalPrice(newTotalPrice);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void UpdateTotalPrice_ShouldThrowBusinessRuleException_WhenBookingIsCancelled()
    {
        // Arrange
        var booking = CreateBooking();
        booking.SetAsCancelled("Guest requested cancellation");
        var newTotalPrice = new Money(320m, CurrencyCode.USD);

        // Act
        Action act = () => booking.UpdateTotalPrice(newTotalPrice);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void UpdateDiscountId_ShouldUpdateDiscountId_WhenValueIsValid()
    {
        // Arrange
        var booking = CreateBooking();
        var discountId = Guid.NewGuid();

        // Act
        booking.UpdateDiscountId(discountId);

        // Assert
        booking.DiscountId.Should().Be(discountId);
    }

    [Fact]
    public void UpdateDiscountId_ShouldThrowBusinessRuleException_WhenBookingIsCompleted()
    {
        // Arrange
        var booking = CreateBooking();
        booking.SetAsCompleted();

        // Act
        Action act = () => booking.UpdateDiscountId(Guid.NewGuid());

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void UpdateDiscountId_ShouldThrowBusinessRuleException_WhenBookingIsCancelled()
    {
        // Arrange
        var booking = CreateBooking();
        booking.SetAsCancelled("Guest requested cancellation");

        // Act
        Action act = () => booking.UpdateDiscountId(Guid.NewGuid());

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    private static Booking CreateBooking()
    {
        return new Booking(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            BookingSource.InPerson,
            CreateGuestCount(),
            CreateCheckInOutTimes(),
            CreateTotalPrice(),
            "Late arrival");
    }

    private static CheckInOutTimes CreateCheckInOutTimes()
    {
        return new CheckInOutTimes(
            new DateTime(2026, 4, 1, 14, 0, 0),
            new DateTime(2026, 4, 3, 12, 0, 0));
    }

    private static Money CreateTotalPrice()
    {
        return new Money(250m, CurrencyCode.DOP);
    }

    private static GuestCount CreateGuestCount()
    {
        return new GuestCount(2, 1);
    }
}
