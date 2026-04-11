using BrisaPMS.Domain.Rooms;
using BrisaPMS.Domain.RoomTypes;
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
        var roomType = CreateRoomType();

        // Act
        var result = new Room(
            hotelId,
            "201",
            2,
            RoomAvailabilityStatus.Available,
            RoomHygieneStatus.Clean,
            roomType);

        // Assert
        result.Id.Should().NotBe(Guid.Empty);
        result.HotelId.Should().Be(hotelId);
        result.Number.Should().Be("201");
        result.Floor.Should().Be(2);
        result.AvailabilityStatus.Should().Be(RoomAvailabilityStatus.Available);
        result.HygieneStatus.Should().Be(RoomHygieneStatus.Clean);
        result.LastCleanedAt.Should().BeNull();
        result.LastCleanedBy.Should().BeNull();
        result.NeedsRestocking.Should().BeFalse();
        result.RoomType.Should().BeSameAs(roomType);
    }

    [Fact]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenHotelIdIsEmpty()
    {
        // Arrange
        var hotelId = Guid.Empty;

        // Act
        Action act = () => _ = new Room(
            hotelId,
            "101",
            1,
            RoomAvailabilityStatus.Available,
            RoomHygieneStatus.Clean,
            CreateRoomType());

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
            roomNumber!,
            1,
            RoomAvailabilityStatus.Available,
            RoomHygieneStatus.Clean,
            CreateRoomType());

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
            "101",
            1,
            invalidAvailabilityStatus,
            RoomHygieneStatus.Clean,
            CreateRoomType());

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
            "101",
            1,
            RoomAvailabilityStatus.Available,
            invalidHygieneStatus,
            CreateRoomType());

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void ChangeRoomType_ShouldUpdateRoomType_WhenValueIsValid()
    {
        // Arrange
        var room = CreateRoom();
        var newRoomType = CreateRoomType("Suite Presidencial");

        // Act
        room.ChangeRoomType(newRoomType);

        // Assert
        room.RoomType.Should().BeSameAs(newRoomType);
    }

    [Fact]
    public void ChangeRoomType_ShouldThrowBusinessRuleException_WhenRoomIsOccupied()
    {
        // Arrange
        var room = CreateRoom();
        var newRoomType = CreateRoomType("Suite Presidencial");
        room.UpdateAvailabilityStatus(RoomAvailabilityStatus.Occupied);

        // Act
        Action act = () => room.ChangeRoomType(newRoomType);

        // Assert
        act.Should().Throw<BusinessRuleException>();
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

    [Fact]
    public void UpdateNumber_ShouldThrowBusinessRuleException_WhenRoomIsOccupied()
    {
        // Arrange
        var room = CreateRoom();
        room.UpdateAvailabilityStatus(RoomAvailabilityStatus.Occupied);

        // Act
        Action act = () => room.UpdateNumber("305");

        // Assert
        act.Should().Throw<BusinessRuleException>();
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
    public void UpdateHygieneStatus_ShouldThrowBusinessRuleException_WhenSettingCleanWhileRoomIsOccupied()
    {
        // Arrange
        var room = CreateRoom();
        room.UpdateAvailabilityStatus(RoomAvailabilityStatus.Occupied);

        // Act
        Action act = () => room.UpdateHygieneStatus(RoomHygieneStatus.Clean);

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
            "101",
            1,
            RoomAvailabilityStatus.Available,
            RoomHygieneStatus.Clean,
            CreateRoomType());
    }

    private static RoomType CreateRoomType(string name = "Deluxe Suite")
    {
        return new RoomType(
            name,
            25m,
            2,
            BedType.Queen,
            2,
            1,
            "Spacious suite with ocean view");
    }
}
