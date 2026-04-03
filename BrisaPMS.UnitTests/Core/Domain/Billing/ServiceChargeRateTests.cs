using BrisaPMS.Domain.Billing;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Billing;

public class ServiceChargeRateTests
{
    [Fact]
    public void Constructor_ShouldCreateServiceChargeRate_WhenRateIsValid()
    {
        // Arrange
        var rate = 10.8m;
        
        // Act
        var serviceChargeRate = new ServiceChargeRate(rate);
        
        // Assert
        serviceChargeRate.Rate.Should().Be(rate);
    }

    [Fact]
    public void Constructor_ShouldThrowInvalidServiceChargeRateException_WhenRateIsNegative()
    {
        // Arrange
        var rate = -1.01m;
        
        // Act
        Action act = () => new ServiceChargeRate(rate);
        
        act.Should().Throw<InvalidServiceChargeRateException>();
    }

    [Fact]
    public void Constructor_ShouldThrowInvalidServiceChargeRateException_WhenRateIsOver100()
    {
        // Arrange
        var rate = 101m;
        
        // Act
        Action act = () => new ServiceChargeRate(rate);
        
        // Assert
        act.Should().Throw<InvalidServiceChargeRateException>();
    }
}