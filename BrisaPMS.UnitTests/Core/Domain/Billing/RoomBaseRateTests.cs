using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Shared.Exceptions;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Billing;

public class RoomBaseRateTests
{
    [Fact]
    public void Constructor_CreatesRoomBaseRate_WhenValuesAreValid()
    {
        // Arrange
        var rate = 0.25m;
        
        // Act
        var roomBaseRate = new RoomBaseRate(rate);
        
        // Assert
        roomBaseRate.Rate.Should().Be(rate);
    }
    
    [Fact]
    public void Constructor_ThrowsBusinessRuleException_WhenRateIsNegative()
    {
        // Arrange
        var rate = -0.25m;
        
        // Act
        var act = () => new RoomBaseRate(rate);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }
    
    [Fact]
    public void Constructor_ThrowsBusinessRuleException_WhenRateIsGreaterThan100()
    {
        // Arrange
        var rate = 101m;
        
        // Act
        var act = () => new RoomBaseRate(rate);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }
}