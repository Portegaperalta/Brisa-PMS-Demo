using BrisaPMS.Domain.Rooms;
using BrisaPMS.Domain.Shared.Exceptions;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Rooms;

public class RoomRateTimeIntervalTests
{
  [Fact]
  public void Constructor_ShouldCreateRateTimeInterval_WhenValidFromIsBeforeValidTo()
  {
    // Arrange
    var validFrom = new DateTime(2026, 4, 1, 0, 0, 0);
    var validTo = new DateTime(2026, 4, 30, 0, 0, 0);

    // Act
    var result = new RoomRateTimeInterval(validFrom, validTo);

    // Assert
    result.ValidFrom.Should().Be(validFrom);
    result.ValidTo.Should().Be(validTo);
  }

  [Fact]
  public void Constructor_ShouldCreateRateTimeInterval_WhenValidFromIsEqualToValidTo()
  {
    // Arrange
    var validFrom = new DateTime(2026, 4, 1, 0, 0, 0);
    var validTo = validFrom;

    // Act
    var result = new RoomRateTimeInterval(validFrom, validTo);

    // Assert
    result.ValidFrom.Should().Be(validFrom);
    result.ValidTo.Should().Be(validTo);
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenValidFromIsAfterValidTo()
  {
    // Arrange
    var validFrom = new DateTime(2026, 5, 1, 0, 0, 0);
    var validTo = new DateTime(2026, 4, 1, 0, 0, 0);

    // Act
    Action act = () => _ = new RoomRateTimeInterval(validFrom, validTo);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }
}
