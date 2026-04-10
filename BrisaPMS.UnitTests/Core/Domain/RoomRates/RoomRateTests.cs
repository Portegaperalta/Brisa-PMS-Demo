using BrisaPMS.Domain.RoomRates;
using BrisaPMS.Domain.Rooms;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.RoomRates;

public class RoomRateTests
{
    [Fact]
    public void Constructor_ShouldCreateRoomRate_WhenValuesAreValid()
    {
        // Arrange
        var roomTypeId = Guid.NewGuid();
        var timeInterval = CreateRateTimeInterval();
        var pricePerNight = CreatePricePerNight();

        // Act
        var result = new RoomRate(roomTypeId, "Standard Nightly Rate", RoomRateType.Nightly, pricePerNight, timeInterval);

        // Assert
        result.Id.Should().NotBe(Guid.Empty);
        result.RoomTypeId.Should().Be(roomTypeId);
        result.Name.Should().Be("Standard Nightly Rate");
        result.Type.Should().Be(RoomRateType.Nightly);
        result.PricePerNight.Should().Be(pricePerNight);
        result.TimeInterval.Should().Be(timeInterval);
    }

    [Fact]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenRoomTypeIdIsEmpty()
    {
        // Arrange
        var roomTypeId = Guid.Empty;

        // Act
        Action act = () => _ = new RoomRate(roomTypeId, "Standard Nightly Rate", RoomRateType.Nightly, CreatePricePerNight(), CreateRateTimeInterval());

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenNameIsNullOrWhiteSpace(string? name)
    {
        // Act
        Action act = () => _ = new RoomRate(Guid.NewGuid(), name!, RoomRateType.Nightly, CreatePricePerNight(), CreateRateTimeInterval());

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenTypeIsInvalid()
    {
        // Arrange
        var invalidType = (RoomRateType)999;

        // Act
        Action act = () => _ = new RoomRate(
            Guid.NewGuid(),
            "Standard Nightly Rate",
            invalidType,
            CreatePricePerNight(),
            CreateRateTimeInterval());

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void UpdateName_ShouldUpdateName_WhenValueIsValid()
    {
        // Arrange
        var roomRate = CreateRoomRate();

        // Act
        roomRate.UpdateName("Weekend Premium Rate");

        // Assert
        roomRate.Name.Should().Be("Weekend Premium Rate");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void UpdateName_ShouldThrowEmptyRequiredFieldException_WhenValueIsNullOrWhiteSpace(string? newName)
    {
        // Arrange
        var roomRate = CreateRoomRate();

        // Act
        Action act = () => roomRate.UpdateName(newName!);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void UpdateType_ShouldUpdateType_WhenValueIsValid()
    {
        // Arrange
        var roomRate = CreateRoomRate();

        // Act
        roomRate.UpdateType(RoomRateType.Weekly);

        // Assert
        roomRate.Type.Should().Be(RoomRateType.Weekly);
    }

    [Fact]
    public void UpdateType_ShouldThrowBusinessRuleException_WhenValueIsInvalid()
    {
        // Arrange
        var roomRate = CreateRoomRate();
        var invalidType = (RoomRateType)999;

        // Act
        Action act = () => roomRate.UpdateType(invalidType);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void UpdatePricePerNight_ShouldUpdatePricePerNight_WhenValueIsValid()
    {
        // Arrange
        var roomRate = CreateRoomRate();
        var newPricePerNight = new Money(200m, CurrencyCode.USD);

        // Act
        roomRate.UpdatePricePerNight(newPricePerNight);

        // Assert
        roomRate.PricePerNight.Should().Be(newPricePerNight);
    }

    [Fact]
    public void UpdateRateTimeInterval_ShouldUpdateTimeInterval_WhenValueIsValid()
    {
        // Arrange
        var roomRate = CreateRoomRate();
        var newTimeInterval = new RoomRateTimeInterval(
            new DateTime(2026, 4, 10, 0, 0, 0),
            new DateTime(2026, 4, 20, 0, 0, 0));

        // Act
        roomRate.UpdateRateTimeInterval(newTimeInterval);

        // Assert
        roomRate.TimeInterval.Should().Be(newTimeInterval);
    }

    private static RoomRate CreateRoomRate()
    {
        return new RoomRate(
            Guid.NewGuid(),
            "Standard Nightly Rate",
            RoomRateType.Nightly,
            CreatePricePerNight(),
            CreateRateTimeInterval());
    }

    private static RoomRateTimeInterval CreateRateTimeInterval()
    {
        return new RoomRateTimeInterval(
            new DateTime(2026, 4, 1, 0, 0, 0),
            new DateTime(2026, 4, 30, 0, 0, 0));
    }

    private static Money CreatePricePerNight()
    {
        return new Money(150m, CurrencyCode.DOP);
    }
}
