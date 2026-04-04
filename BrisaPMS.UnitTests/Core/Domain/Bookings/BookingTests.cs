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
    var result = new Booking(hotelId, roomId, guestId, "Direct", guestCount, checkInOutTimes, totalPrice, "Late arrival", discountId);

    // Assert
    result.Id.Should().NotBe(Guid.Empty);
    result.HotelId.Should().Be(hotelId);
    result.RoomId.Should().Be(roomId);
    result.GuestId.Should().Be(guestId);
    result.Source.Should().Be("Direct");
    result.GuestCount.NumberOfAdults.Should().Be(guestCount.NumberOfAdults);
    result.GuestCount.NumberOfChildren.Should().Be(guestCount.NumberOfChildren);
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
        "Online Travel Agency",
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
    // Arrange
    var hotelId = Guid.Empty;

    // Act
    Action act = () => _ = new Booking(hotelId, Guid.NewGuid(), Guid.NewGuid(), "Direct", CreateGuestCount(), CreateCheckInOutTimes(), CreateTotalPrice());

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Fact]
  public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenRoomIdIsEmpty()
  {
    // Arrange
    var roomId = Guid.Empty;

    // Act
    Action act = () => _ = new Booking(Guid.NewGuid(), roomId, Guid.NewGuid(), "Direct", CreateGuestCount(), CreateCheckInOutTimes(), CreateTotalPrice());

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Fact]
  public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenGuestIdIsEmpty()
  {
    // Arrange
    var guestId = Guid.Empty;

    // Act
    Action act = () => _ = new Booking(Guid.NewGuid(), Guid.NewGuid(), guestId, "Direct", CreateGuestCount(), CreateCheckInOutTimes(), CreateTotalPrice());

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Theory]
  [InlineData(null)]
  [InlineData("  ")]
  public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenSourceIsNullOrWhiteSpace(string? bookingSource)
  {
    // Act
    Action act = () => _ = new Booking(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), bookingSource!, CreateGuestCount(), CreateCheckInOutTimes(), CreateTotalPrice());

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenNumberOfAdultsIsZeroOrLess()
  {
    // Arrange
    const int numberOfAdults = 0;

    // Act
    Action act = () => _ = new GuestCount(numberOfAdults, 0);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenNumberOfChildrenIsNegative()
  {
    // Arrange
    const int numberOfChildren = -1;

    // Act
    Action act = () => _ = new GuestCount(2, numberOfChildren);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void UpdateSource_ShouldUpdateSource_WhenValueIsValid()
  {
    // Arrange
    var booking = CreateBooking();

    // Act
    booking.UpdateSource("Walk-in");

    // Assert
    booking.Source.Should().Be("Walk-in");
  }

  [Theory]
  [InlineData(null)]
  [InlineData("  ")]
  public void UpdateSource_ShouldThrowEmptyRequiredFieldException_WhenValueIsNullOrWhiteSpace(string? newBookingSource)
  {
    // Arrange
    var booking = CreateBooking();

    // Act
    Action act = () => booking.UpdateSource(newBookingSource!);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
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
  public void UpdateGuestCount_ShouldThrowBusinessRuleException_WhenNumberOfAdultsIsZeroOrLess()
  {
    // Act
    Action act = () => _ = new GuestCount(0, 1);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void UpdateGuestCount_ShouldThrowBusinessRuleException_WhenNumberOfChildrenIsNegative()
  {
    // Act
    Action act = () => _ = new GuestCount(2, -1);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void UpdateCheckInOutTimes_ShouldUpdateCheckInOutTimes_WhenValueIsValid()
  {
    // Arrange
    var booking = CreateBooking();
    var newCheckInOutTimes = new CheckInOutTimes(new DateTime(2026, 4, 3, 15, 0, 0), new DateTime(2026, 4, 5, 12, 0, 0));

    // Act
    booking.UpdateCheckInOutTimes(newCheckInOutTimes);

    // Assert
    booking.CheckInOutTimes.Should().Be(newCheckInOutTimes);
  }

  [Fact]
  public void UpdateSpecialRequests_ShouldUpdateSpecialRequests()
  {
    // Arrange
    var booking = CreateBooking();

    // Act
    booking.UpdateSpecialRequests("High floor room");

    // Assert
    booking.SpecialRequests.Should().Be("High floor room");
  }

  [Fact]
  public void UpdateCancellationReason_ShouldUpdateCancellationReason_WhenValueIsValid()
  {
    // Arrange
    var booking = CreateBooking();

    // Act
    booking.UpdateCancellationReason("Guest requested cancellation");

    // Assert
    booking.CancellationReason.Should().Be("Guest requested cancellation");
  }

  [Theory]
  [InlineData(null)]
  [InlineData("  ")]
  public void UpdateCancellationReason_ShouldThrowEmptyRequiredFieldException_WhenValueIsNullOrWhiteSpace(string? newCancellationReason)
  {
    // Arrange
    var booking = CreateBooking();

    // Act
    Action act = () => booking.UpdateCancellationReason(newCancellationReason!);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
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

  private static Booking CreateBooking()
  {
    return new Booking(
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        "Direct",
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
