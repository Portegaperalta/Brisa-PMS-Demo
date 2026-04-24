using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Stays;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Stays;

public class StayTests
{
  [Fact]
  public void Constructor_ShouldCreateStay_WhenValuesAreValid()
  {
    // Arrange
    var guestId = Guid.NewGuid();
    var bookingId = Guid.NewGuid();

    // Act
    var result = new Stay(guestId, bookingId);

    // Assert
    result.Id.Should().NotBe(Guid.Empty);
    result.GuestId.Should().Be(guestId);
    result.BookingId.Should().Be(bookingId);
    result.TimeInterval.ActualCheckIn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    result.TimeInterval.ActualCheckOut.Should().BeNull();
    result.NightCount.Should().Be(0);
    result.Status.Should().Be(StayStatus.InProgress);
  }

  [Fact]
  public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenGuestIdIsEmpty()
  {
    // Arrange
    var guestId = Guid.Empty;

    // Act
    Action act = () => _ = new Stay(guestId, Guid.NewGuid());

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Fact]
  public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenBookingIdIsEmpty()
  {
    // Arrange
    var bookingId = Guid.Empty;

    // Act
    Action act = () => _ = new Stay(Guid.NewGuid(), bookingId);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Fact]
  public void IncreaseNightCount_ShouldIncrementNightCount_WhenStayIsInProgress()
  {
    // Arrange
    var stay = CreateStay();

    // Act
    stay.IncreaseNightCount();
    stay.IncreaseNightCount();

    // Assert
    stay.NightCount.Should().Be(2);
  }

  [Fact]
  public void IncreaseNightCount_ShouldThrowBusinessRuleException_WhenStayIsComplete()
  {
    // Arrange
    var stay = CreateStay();
    stay.SetAsComplete();

    // Act
    Action act = () => stay.IncreaseNightCount();

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void SetAsComplete_ShouldSetStatusToCompleteAndActualCheckOut()
  {
    // Arrange
    var stay = CreateStay();
    var actualCheckIn = stay.TimeInterval.ActualCheckIn;

    // Act
    stay.SetAsComplete();

    // Assert
    stay.Status.Should().Be(StayStatus.Complete);
    stay.TimeInterval.ActualCheckIn.Should().Be(actualCheckIn);
    stay.TimeInterval.ActualCheckOut.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
  }

  [Fact]
  public void SetAsComplete_ShouldKeepStayComplete_WhenCalledMoreThanOnce()
  {
    // Arrange
    var stay = CreateStay();
    stay.SetAsComplete();

    // Act
    var act = () => stay.SetAsComplete();

    // Assert
    act.Should().NotThrow();
    stay.Status.Should().Be(StayStatus.Complete);
    stay.TimeInterval.ActualCheckOut.Should().NotBeNull();
  }

  private static Stay CreateStay()
  {
    return new Stay(Guid.NewGuid(), Guid.NewGuid());
  }
}
