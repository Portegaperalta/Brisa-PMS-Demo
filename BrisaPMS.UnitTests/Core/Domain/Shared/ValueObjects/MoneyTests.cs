using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Shared.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Constructor_ShouldCreateMoney_WhenAmountAndCurrencyAreValid()
    {
        // Arrange
        const decimal amount = 150.75m;

        // Act
        var result = new Money(amount, CurrencyCode.USD);

        // Assert
        result.Amount.Should().Be(amount);
        result.CurrencyCode.Should().Be(CurrencyCode.USD);
    }

    [Fact]
    public void Constructor_ShouldCreateMoney_WhenCurrencyIsNotProvided()
    {
        // Arrange
        const decimal amount = 99.99m;

        // Act
        var result = new Money(amount);

        // Assert
        result.Amount.Should().Be(amount);
        result.CurrencyCode.Should().Be(CurrencyCode.DOP);
    }

    [Fact]
    public void Constructor_ShouldCreateMoney_WhenAmountIsZero()
    {
        // Arrange
        const decimal amount = 0m;

        // Act
        var result = new Money(amount, CurrencyCode.EUR);

        // Assert
        result.Amount.Should().Be(amount);
        result.CurrencyCode.Should().Be(CurrencyCode.EUR);
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenAmountIsNegative()
    {
        // Arrange
        const decimal amount = -0.01m;

        // Act
        Action act = () => _ = new Money(amount, CurrencyCode.USD);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenCurrencyCodeIsInvalid()
    {
        // Arrange
        var invalidCurrencyCode = (CurrencyCode)999;

        // Act
        Action act = () => _ = new Money(10m, invalidCurrencyCode);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }
}
