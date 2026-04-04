using BrisaPMS.Domain.Inventory;
using BrisaPMS.Domain.Shared.Exceptions;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Inventory;

public class StockThresholdTests
{
    [Fact]
    public void Constructor_ShouldCreateStockThreshold_WhenValuesAreValid()
    {
        // Arrange
        const decimal minStockThreshold = 10m;
        const decimal maxStockThreshold = 100m;

        // Act
        var result = new StockThreshold(minStockThreshold, maxStockThreshold);

        // Assert
        result.MinStockThreshold.Should().Be(minStockThreshold);
        result.MaxStockThreshold.Should().Be(maxStockThreshold);
    }

    [Fact]
    public void Constructor_ShouldCreateStockThreshold_WhenValuesAreZero()
    {
        // Arrange
        const decimal minStockThreshold = 0m;
        const decimal maxStockThreshold = 0m;

        // Act
        var result = new StockThreshold(minStockThreshold, maxStockThreshold);

        // Assert
        result.MinStockThreshold.Should().Be(minStockThreshold);
        result.MaxStockThreshold.Should().Be(maxStockThreshold);
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenMinStockThresholdIsNegative()
    {
        // Arrange
        const decimal minStockThreshold = -1m;
        const decimal maxStockThreshold = 100m;

        // Act
        Action act = () => _ = new StockThreshold(minStockThreshold, maxStockThreshold);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenMaxStockThresholdIsNegative()
    {
        // Arrange
        const decimal minStockThreshold = 10m;
        const decimal maxStockThreshold = -1m;

        // Act
        Action act = () => _ = new StockThreshold(minStockThreshold, maxStockThreshold);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }
}
