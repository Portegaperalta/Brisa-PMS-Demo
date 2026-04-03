using BrisaPMS.Domain.Discount;
using BrisaPMS.Domain.Discounts;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Discounts;

public class DiscountTests
{
  [Fact]
  public void Constructor_ShouldCreateDiscount_WhenValuesAreValid()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var value = CreateMoney(15m);
    var timeInterval = CreateDiscountTimeInterval();

    // Act
    var result = new Discount(hotelId, "Early Booking", "Percentage", value, timeInterval, true);

    // Assert
    result.Id.Should().NotBe(Guid.Empty);
    result.HotelId.Should().Be(hotelId);
    result.Name.Should().Be("Early Booking");
    result.Type.Should().Be("Percentage");
    result.Value.Should().Be(value);
    result.TimeInterval.Should().Be(timeInterval);
    result.IsActive.Should().BeTrue();
  }

  [Fact]
  public void Constructor_ShouldCreateDiscount_WhenInactive()
  {
    // Arrange
    var timeInterval = CreateDiscountTimeInterval();

    // Act
    var result = new Discount(Guid.NewGuid(), "Corporate Rate", "Fixed", CreateMoney(25m), timeInterval, false);

    // Assert
    result.IsActive.Should().BeFalse();
  }

  [Fact]
  public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenHotelIdIsEmpty()
  {
    // Arrange
    var hotelId = Guid.Empty;

    // Act
    Action act = () => _ = new Discount(hotelId, "Early Booking", "Percentage", CreateMoney(15m), CreateDiscountTimeInterval(), true);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenNameIsNullOrEmpty(string? name)
  {
    // Act
    Action act = () => _ = new Discount(
        Guid.NewGuid(),
        name!,
        "Percentage",
        CreateMoney(15m),
        CreateDiscountTimeInterval(),
        true);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenTypeIsNullOrEmpty(string? type)
  {
    // Act
    Action act = () => _ = new Discount(
        Guid.NewGuid(),
        "Early Booking",
        type!,
        CreateMoney(15m),
        CreateDiscountTimeInterval(),
        true);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Fact]
  public void UpdateName_ShouldUpdateName_WhenValueIsValid()
  {
    // Arrange
    var discount = CreateDiscount();

    // Act
    discount.UpdateName("Last Minute");

    // Assert
    discount.Name.Should().Be("Last Minute");
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  public void UpdateName_ShouldThrowEmptyRequiredFieldException_WhenValueIsNullOrEmpty(string? newName)
  {
    // Arrange
    var discount = CreateDiscount();

    // Act
    Action act = () => discount.UpdateName(newName!);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Fact]
  public void UpdateType_ShouldUpdateType_WhenValueIsValid()
  {
    // Arrange
    var discount = CreateDiscount();

    // Act
    discount.UpdateType("Fixed");

    // Assert
    discount.Type.Should().Be("Fixed");
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  public void UpdateType_ShouldThrowEmptyRequiredFieldException_WhenValueIsNullOrEmpty(string? newType)
  {
    // Arrange
    var discount = CreateDiscount();

    // Act
    Action act = () => discount.UpdateType(newType!);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Fact]
  public void UpdateValue_ShouldUpdateValue_WhenValueIsValid()
  {
    // Arrange
    var discount = CreateDiscount();
    var newValue = new Money(30m, CurrencyCode.USD);

    // Act
    discount.UpdateValue(newValue);

    // Assert
    discount.Value.Should().Be(newValue);
  }

  [Fact]
  public void UpdateTimeInterval_ShouldUpdateTimeInterval_WhenDiscountIsActive()
  {
    // Arrange
    var discount = CreateDiscount();
    var newTimeInterval = new DiscountTimeInterval(
        new DateTime(2026, 5, 1, 0, 0, 0),
        new DateTime(2026, 5, 31, 0, 0, 0));

    // Act
    discount.UpdateTimeInterval(newTimeInterval);

    // Assert
    discount.TimeInterval.Should().Be(newTimeInterval);
  }

  [Fact]
  public void UpdateTimeInterval_ShouldThrowBusinessRuleException_WhenDiscountIsInactive()
  {
    // Arrange
    var discount = CreateDiscount(isActive: false);
    var newTimeInterval = new DiscountTimeInterval(
        new DateTime(2026, 5, 1, 0, 0, 0),
        new DateTime(2026, 5, 31, 0, 0, 0));

    // Act
    Action act = () => discount.UpdateTimeInterval(newTimeInterval);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void SetAsActive_ShouldSetIsActiveToTrue()
  {
    // Arrange
    var discount = CreateDiscount(isActive: false);

    // Act
    discount.SetAsActive();

    // Assert
    discount.IsActive.Should().BeTrue();
  }

  [Fact]
  public void SetAsInactive_ShouldSetIsActiveToFalse()
  {
    // Arrange
    var discount = CreateDiscount();

    // Act
    discount.SetAsInactive();

    // Assert
    discount.IsActive.Should().BeFalse();
  }

  private static Discount CreateDiscount(bool isActive = true)
  {
    return new Discount(
        Guid.NewGuid(),
        "Early Booking",
        "Percentage",
        CreateMoney(15m),
        CreateDiscountTimeInterval(),
        isActive);
  }

  private static DiscountTimeInterval CreateDiscountTimeInterval()
  {
    return new DiscountTimeInterval(
        new DateTime(2026, 4, 1, 0, 0, 0),
        new DateTime(2026, 4, 30, 0, 0, 0));
  }

  private static Money CreateMoney(decimal amount)
  {
    return new Money(amount, CurrencyCode.DOP);
  }
}
