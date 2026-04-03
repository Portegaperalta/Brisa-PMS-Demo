using BrisaPMS.Domain.Booking;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Booking;

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

    // Act
    var result = new Booking(hotelId, roomId, guestId, "Direct", 2, 1, checkInOutTimes, 250m, "Late arrival", discountId);

    // Assert
    result.Id.Should().NotBe(Guid.Empty);
    result.HotelId.Should().Be(hotelId);
    result.RoomId.Should().Be(roomId);
    result.GuestId.Should().Be(guestId);
    result.Source.Should().Be("Direct");
    result.NumberOfAdults.Should().Be(2);
    result.NumberOfChildren.Should().Be(1);
    result.CheckInOutTimes.Should().Be(checkInOutTimes);
    result.SpecialRequests.Should().Be("Late arrival");
    result.Status.Should().Be(BookingStatus.Pending);
    result.CancellationReason.Should().BeNull();
    result.TotalPrice.Should().Be(250m);
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
    var result = new Booking(hotelId, roomId, guestId, "Online Travel Agency", 2, 0, CreateCheckInOutTimes(), 180m);

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
    Action act = () => _ = new Booking(hotelId, Guid.NewGuid(), Guid.NewGuid(), "Direct", 2, 0, CreateCheckInOutTimes(), 100m);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Fact]
  public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenRoomIdIsEmpty()
  {
    // Arrange
    var roomId = Guid.Empty;

    // Act
    Action act = () => _ = new Booking(Guid.NewGuid(), roomId, Guid.NewGuid(), "Direct", 2, 0, CreateCheckInOutTimes(), 100m);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Fact]
  public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenGuestIdIsEmpty()
  {
    // Arrange
    var guestId = Guid.Empty;

    // Act
    Action act = () => _ = new Booking(Guid.NewGuid(), Guid.NewGuid(), guestId, "Direct", 2, 0, CreateCheckInOutTimes(), 100m);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Theory]
  [InlineData(null)]
  [InlineData("  ")]
  public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenSourceIsNullOrWhiteSpace(string? bookingSource)
  {
    // Act
    Action act = () => _ = new Booking(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), bookingSource!, 2, 0, CreateCheckInOutTimes(), 100m);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenNumberOfAdultsIsZeroOrLess()
  {
    // Arrange
    const int numberOfAdults = 0;

    // Act
    Action act = () => _ = new Booking(
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        "Direct",
        numberOfAdults,
        0,
        CreateCheckInOutTimes(),
        100m);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenNumberOfChildrenIsNegative()
  {
    // Arrange
    const int numberOfChildren = -1;

    // Act
    Action act = () => _ = new Booking(
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        "Direct",
        2,
        numberOfChildren,
        CreateCheckInOutTimes(),
        100m);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenTotalPriceIsNegative()
  {
    // Arrange
    const decimal totalPrice = -1m;

    // Act
    Action act = () => _ = new Booking(
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        "Direct",
        2,
        0,
        CreateCheckInOutTimes(),
        totalPrice);

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
  public void UpdateNumberOfAdults_ShouldUpdateNumberOfAdults_WhenValueIsValid()
  {
    // Arrange
    var booking = CreateBooking();

    // Act
    booking.UpdateNumberOfAdults(3);

    // Assert
    booking.NumberOfAdults.Should().Be(3);
  }

  [Fact]
  public void UpdateNumberOfAdults_ShouldThrowBusinessRuleException_WhenValueIsZeroOrLess()
  {
    // Arrange
    var booking = CreateBooking();

    // Act
    Action act = () => booking.UpdateNumberOfAdults(0);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void UpdateNumberOfChildren_ShouldUpdateNumberOfChildren_WhenValueIsValid()
  {
    // Arrange
    var booking = CreateBooking();

    // Act
    booking.UpdateNumberOfChildren(2);

    // Assert
    booking.NumberOfChildren.Should().Be(2);
  }

  [Fact]
  public void UpdateNumberOfChildren_ShouldThrowBusinessRuleException_WhenValueIsNegative()
  {
    // Arrange
    var booking = CreateBooking();

    // Act
    Action act = () => booking.UpdateNumberOfChildren(-1);

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

    // Act
    booking.UpdateTotalPrice(320m);

    // Assert
    booking.TotalPrice.Should().Be(320m);
  }

  [Fact]
  public void UpdateTotalPrice_ShouldThrowBusinessRuleException_WhenValueIsNegative()
  {
    // Arrange
    var booking = CreateBooking();

    // Act
    Action act = () => booking.UpdateTotalPrice(-1m);

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

  private static Booking CreateBooking()
  {
    return new Booking(
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        "Direct",
        2,
        1,
        CreateCheckInOutTimes(),
        250m,
        "Late arrival");
  }

  private static CheckInOutTimes CreateCheckInOutTimes()
  {
    return new CheckInOutTimes(
        new DateTime(2026, 4, 1, 14, 0, 0),
        new DateTime(2026, 4, 3, 12, 0, 0));
  }
}
