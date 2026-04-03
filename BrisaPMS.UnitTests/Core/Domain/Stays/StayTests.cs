using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Stay;
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
    result.ActualCheckIn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    result.ActualCheckOut.Should().BeNull();
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
  public void IncreaseNightCount_ShouldIncrementNightCount()
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
  public void IncreaseNightCount_ShouldThrowBusinessRuleException_WhenStayIsCancelled()
  {
    // Arrange
    var stay = CreateStay();
    stay.SetAsCancelled();

    // Act
    Action act = () => stay.IncreaseNightCount();

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void SetAsComplete_ShouldSetStatusToCompleteAndActualCheckOut_WhenStayIsNotCancelled()
  {
    // Arrange
    var stay = CreateStay();

    // Act
    stay.SetAsComplete();

    // Assert
    stay.Status.Should().Be(StayStatus.Complete);
    stay.ActualCheckOut.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
  }

  [Fact]
  public void SetAsComplete_ShouldThrowBusinessRuleException_WhenStayIsCancelled()
  {
    // Arrange
    var stay = CreateStay();
    stay.SetAsCancelled();

    // Act
    Action act = () => stay.SetAsComplete();

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void SetAsCancelled_ShouldSetStatusToCancelled_WhenStayIsNotComplete()
  {
    // Arrange
    var stay = CreateStay();

    // Act
    stay.SetAsCancelled();

    // Assert
    stay.Status.Should().Be(StayStatus.Cancelled);
  }

  [Fact]
  public void SetAsCancelled_ShouldThrowBusinessRuleException_WhenStayIsComplete()
  {
    // Arrange
    var stay = CreateStay();
    stay.SetAsComplete();

    // Act
    Action act = () => stay.SetAsCancelled();

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  private static Stay CreateStay()
  {
    return new Stay(Guid.NewGuid(), Guid.NewGuid());
  }
}
