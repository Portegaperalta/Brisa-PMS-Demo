using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.ValueObjects;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.ValueObjects;

public class CheckInCheckOutTimesTests
{
    [Fact]
    public void Constructor_ShouldCreateCheckInOutTimes_WhenCheckInTimeIsBeforeCheckOutTime()
    {
        // Arrange
        var checkInTime = new DateTime(2026, 3, 28, 14, 0, 0);
        var checkOutTime = new DateTime(2026, 3, 29, 12, 0, 0);

        // Act
        var result = new CheckInOutTimes(checkInTime, checkOutTime);

        // Assert
        result.CheckInTime.Should().Be(checkInTime);
        result.CheckOutTime.Should().Be(checkOutTime);
    }

    [Fact]
    public void Constructor_ShouldCreateCheckInOutTimes_WhenCheckInTimeIsEqualToCheckOutTime()
    {
        // Arrange
        var checkInTime = new DateTime(2026, 3, 28, 14, 0, 0);
        var checkOutTime = checkInTime;

        // Act
        var result = new CheckInOutTimes(checkInTime, checkOutTime);

        // Assert
        result.CheckInTime.Should().Be(checkInTime);
        result.CheckOutTime.Should().Be(checkOutTime);
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenCheckInTimeIsGreaterThanCheckOutTime()
    {
        // Arrange
        var checkInTime = new DateTime(2026, 3, 29, 12, 0, 0);
        var checkOutTime = new DateTime(2026, 3, 28, 14, 0, 0);

        // Act
        Action act = () => _ = new CheckInOutTimes(checkInTime, checkOutTime);

        // Assert
        var result = act.Should().Throw<BusinessRuleException>();
    }
}
