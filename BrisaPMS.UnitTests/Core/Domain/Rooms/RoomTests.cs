using BrisaPMS.Domain.Rooms;
using BrisaPMS.Domain.Shared.Exceptions;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Rooms;

public class RoomTests
{
    [Fact]
    public void Constructor_ShouldCreateRoom_WhenValuesAreValid()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomTypeId = Guid.NewGuid();

        // Act
        var result = new Room(
            hotelId,
            roomTypeId,
            "201",
            2,
            RoomAvailabilityStatus.Available,
            RoomHygieneStatus.Clean);

        // Assert
        result.Id.Should().NotBe(Guid.Empty);
        result.HotelId.Should().Be(hotelId);
        result.RoomTypeId.Should().Be(roomTypeId);
        result.Number.Should().Be("201");
        result.Floor.Should().Be(2);
        result.AvailabilityStatus.Should().Be(RoomAvailabilityStatus.Available);
        result.HygieneStatus.Should().Be(RoomHygieneStatus.Clean);
        result.LastCleanedAt.Should().BeNull();
        result.LastCleanedBy.Should().BeNull();
        result.NeedsRestocking.Should().BeFalse();
    }

    [Fact]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenHotelIdIsEmpty()
    {
        // Arrange
        var hotelId = Guid.Empty;

        // Act
        Action act = () => _ = new Room(
            hotelId,
            Guid.NewGuid(),
            "101",
            1,
            RoomAvailabilityStatus.Available,
            RoomHygieneStatus.Clean);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenRoomTypeIdIsEmpty()
    {
        // Arrange
        var roomTypeId = Guid.Empty;

        // Act
        Action act = () => _ = new Room(
            Guid.NewGuid(),
            roomTypeId,
            "101",
            1,
            RoomAvailabilityStatus.Available,
            RoomHygieneStatus.Clean);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenRoomNumberIsNullOrWhiteSpace(string? roomNumber)
    {
        // Act
        Action act = () => _ = new Room(
            Guid.NewGuid(),
            Guid.NewGuid(),
            roomNumber!,
            1,
            RoomAvailabilityStatus.Available,
            RoomHygieneStatus.Clean);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenAvailabilityStatusIsInvalid()
    {
        // Arrange
        var invalidAvailabilityStatus = (RoomAvailabilityStatus)999;

        // Act
        Action act = () => _ = new Room(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "101",
            1,
            invalidAvailabilityStatus,
            RoomHygieneStatus.Clean);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenHygieneStatusIsInvalid()
    {
        // Arrange
        var invalidHygieneStatus = (RoomHygieneStatus)999;

        // Act
        Action act = () => _ = new Room(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "101",
            1,
            RoomAvailabilityStatus.Available,
            invalidHygieneStatus);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void UpdateRoomType_ShouldUpdateRoomTypeId_WhenValueIsValid()
    {
        // Arrange
        var room = CreateRoom();
        var newRoomTypeId = Guid.NewGuid();

        // Act
        room.UpdateRoomType(newRoomTypeId);

        // Assert
        room.RoomTypeId.Should().Be(newRoomTypeId);
    }

    [Fact]
    public void UpdateRoomType_ShouldThrowEmptyRequiredFieldException_WhenValueIsEmpty()
    {
        // Arrange
        var room = CreateRoom();

        // Act
        Action act = () => room.UpdateRoomType(Guid.Empty);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void UpdateNumber_ShouldUpdateNumber_WhenValueIsValid()
    {
        // Arrange
        var room = CreateRoom();

        // Act
        room.UpdateNumber("305");

        // Assert
        room.Number.Should().Be("305");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    public void UpdateNumber_ShouldThrowEmptyRequiredFieldException_WhenValueIsNullOrWhiteSpace(string? roomNumber)
    {
        // Arrange
        var room = CreateRoom();

        // Act
        Action act = () => room.UpdateNumber(roomNumber!);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void UpdateAvailabilityStatus_ShouldUpdateAvailabilityStatus_WhenValueIsValid()
    {
        // Arrange
        var room = CreateRoom();

        // Act
        room.UpdateAvailabilityStatus(RoomAvailabilityStatus.Occupied);

        // Assert
        room.AvailabilityStatus.Should().Be(RoomAvailabilityStatus.Occupied);
    }

    [Fact]
    public void UpdateAvailabilityStatus_ShouldThrowBusinessRuleException_WhenValueIsInvalid()
    {
        // Arrange
        var room = CreateRoom();
        var invalidAvailabilityStatus = (RoomAvailabilityStatus)999;

        // Act
        Action act = () => room.UpdateAvailabilityStatus(invalidAvailabilityStatus);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void UpdateHygieneStatus_ShouldUpdateHygieneStatus_WhenValueIsValid()
    {
        // Arrange
        var room = CreateRoom();

        // Act
        room.UpdateHygieneStatus(RoomHygieneStatus.Dirty);

        // Assert
        room.HygieneStatus.Should().Be(RoomHygieneStatus.Dirty);
    }

    [Fact]
    public void UpdateHygieneStatus_ShouldThrowBusinessRuleException_WhenValueIsInvalid()
    {
        // Arrange
        var room = CreateRoom();
        var invalidHygieneStatus = (RoomHygieneStatus)999;

        // Act
        Action act = () => room.UpdateHygieneStatus(invalidHygieneStatus);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void UpdateLastCleanedAt_ShouldSetLastCleanedAt()
    {
        // Arrange
        var room = CreateRoom();

        // Act
        room.UpdateLastCleanedAt();

        // Assert
        room.LastCleanedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void UpdateLastCleanedBy_ShouldSetLastCleanedBy_WhenUserIdIsValid()
    {
        // Arrange
        var room = CreateRoom();
        var userId = Guid.NewGuid();

        // Act
        room.UpdateLastCleanedBy(userId);

        // Assert
        room.LastCleanedBy.Should().Be(userId);
    }

    [Fact]
    public void UpdateLastCleanedBy_ShouldThrowEmptyRequiredFieldException_WhenUserIdIsEmpty()
    {
        // Arrange
        var room = CreateRoom();

        // Act
        Action act = () => room.UpdateLastCleanedBy(Guid.Empty);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void SetAsPendingRestocking_ShouldSetNeedsRestockingToTrue()
    {
        // Arrange
        var room = CreateRoom();

        // Act
        room.SetAsPendingRestocking();

        // Assert
        room.NeedsRestocking.Should().BeTrue();
    }

    [Fact]
    public void SetAsRestocked_ShouldSetNeedsRestockingToFalse()
    {
        // Arrange
        var room = CreateRoom();
        room.SetAsPendingRestocking();

        // Act
        room.SetAsRestocked();

        // Assert
        room.NeedsRestocking.Should().BeFalse();
    }

    private static Room CreateRoom()
    {
        return new Room(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "101",
            1,
            RoomAvailabilityStatus.Available,
            RoomHygieneStatus.Clean);
    }
}
