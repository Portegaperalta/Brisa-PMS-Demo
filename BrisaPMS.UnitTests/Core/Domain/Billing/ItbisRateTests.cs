using BrisaPMS.Domain.Billing;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Billing;

public class ItbisRateTests
{
    [Fact]
    public void Constructor_ShouldCreateItbisRate_WhenRateIsValid()
    {
        // Arrange
        var rate = 0.18m;
        
        // Act
        var itbisRate = new ItbisRate(rate);
        
        // Assert
        itbisRate.Rate.Should().Be(rate);
    }

    [Fact]
    public void Constructor_ShouldThrowInvalidItbisRateException_WhenRateIsNegative()
    {
        // Arrange
        var rate = -0.18m;
        
        // Act
        Action act = () => new ItbisRate(rate);
        
        // Assert
        act.Should().Throw<InvalidItbisRateException>();
    }
    
    [Fact]
    public void Constructor_ShouldThrowInvalidItbisRateException_WhenRateIsGreaterThan100()
    {
        // Arrange
        var rate = 101m;
        
        // Act
        Action act = () => new ItbisRate(rate);
        
        // Assert
        act.Should().Throw<InvalidItbisRateException>();
    }
}