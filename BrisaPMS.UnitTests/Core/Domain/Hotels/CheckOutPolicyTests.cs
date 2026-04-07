using BrisaPMS.Domain.Hotels;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Hotels;

public class CheckOutPolicyTests
{
    [Fact]
    public void Constructor_ShouldCreateCheckOutPolicy_WhenValuesAreValid()
    {
        // Arrange
        var checkInTime = new TimeOnly(12, 0, 0);
        var checkOutTime = new TimeOnly(15, 0, 0);
        
        // Act
        var checkOutPolicy = new CheckOutPolicy(checkInTime, checkOutTime);
        
        // Assert
        checkOutPolicy.CheckInTime.Should().Be(checkInTime);
        checkOutPolicy.CheckOutTime.Should().Be(checkOutTime);
    }
}