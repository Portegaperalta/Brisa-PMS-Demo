using BrisaPMS.Domain.Entities;
using BrisaPMS.Domain.Enums;
using BrisaPMS.Domain.Exceptions;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Entities;

public class RoomTests
{
  [Fact]
  public void Constructor_ShouldCreateRoom_WhenValuesAreValid()
  {
    // Arrange
    var hotelId = Guid.NewGuid();

    // Act
    var result = new Room(hotelId, RoomType.Deluxe, "201", 2, 2, AvailabilityStatus.Available, HygieneStatus.Clean);

    // Assert
    result.Id.Should().NotBe(Guid.Empty);
    result.HotelId.Should().Be(hotelId);
    result.RoomType.Should().Be(RoomType.Deluxe);
    result.Number.Should().Be("201");
    result.Floor.Should().Be(2);
    result.TotalBeds.Should().Be(2);
    result.AvailabilityStatus.Should().Be(AvailabilityStatus.Available);
    result.HygieneStatus.Should().Be(HygieneStatus.Clean);
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
    Action act = () => _ = new Room(hotelId, RoomType.Standard, "101", 1, 1, AvailabilityStatus.Available, HygieneStatus.Clean);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenRoomTypeIsInvalid()
  {
    // Arrange
    var invalidRoomType = (RoomType)999;

    // Act
    Action act = () => _ = new Room(Guid.NewGuid(), invalidRoomType, "101", 1, 1, AvailabilityStatus.Available, HygieneStatus.Clean);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Theory]
  [InlineData(null)]
  [InlineData("  ")]
  public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenRoomNumberIsNullOrWhiteSpace(string? roomNumber)
  {
    // Act
    Action act = () => _ = new Room(Guid.NewGuid(), RoomType.Standard, roomNumber!, 1, 1, AvailabilityStatus.Available, HygieneStatus.Clean);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Theory]
  [InlineData(0)]
  [InlineData(-1)]
  public void Constructor_ShouldThrowBusinessRuleException_WhenTotalBedsIsZeroOrLess(int totalBeds)
  {
    // Act
    Action act = () => _ = new Room(Guid.NewGuid(), RoomType.Standard, "101", 1, totalBeds, AvailabilityStatus.Available, HygieneStatus.Clean);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenAvailabilityStatusIsInvalid()
  {
    // Arrange
    var invalidAvailabilityStatus = (AvailabilityStatus)999;

    // Act
    Action act = () => _ = new Room(Guid.NewGuid(), RoomType.Standard, "101", 1, 1, invalidAvailabilityStatus, HygieneStatus.Clean);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenHygieneStatusIsInvalid()
  {
    // Arrange
    var invalidHygieneStatus = (HygieneStatus)999;

    // Act
    Action act = () => _ = new Room(Guid.NewGuid(), RoomType.Standard, "101", 1, 1, AvailabilityStatus.Available, invalidHygieneStatus);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void ChangeRoomType_ShouldUpdateRoomType_WhenValueIsValid()
  {
    // Arrange
    var room = CreateRoom();

    // Act
    room.ChangeRoomType(RoomType.Suite);

    // Assert
    room.RoomType.Should().Be(RoomType.Suite);
  }

  [Fact]
  public void ChangeRoomType_ShouldThrowBusinessRuleException_WhenValueIsInvalid()
  {
    // Arrange
    var room = CreateRoom();
    var invalidRoomType = (RoomType)999;

    // Act
    Action act = () => room.ChangeRoomType(invalidRoomType);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void ChangeNumber_ShouldUpdateNumber_WhenValueIsValid()
  {
    // Arrange
    var room = CreateRoom();

    // Act
    room.ChangeNumber("305");

    // Assert
    room.Number.Should().Be("305");
  }

  [Theory]
  [InlineData(null)]
  [InlineData("  ")]
  public void ChangeNumber_ShouldThrowEmptyRequiredFieldException_WhenValueIsNullOrWhiteSpace(string? roomNumber)
  {
    // Arrange
    var room = CreateRoom();

    // Act
    Action act = () => room.ChangeNumber(roomNumber!);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Fact]
  public void ChangeTotalBeds_ShouldUpdateTotalBeds_WhenValueIsValid()
  {
    // Arrange
    var room = CreateRoom();

    // Act
    room.ChangeTotalBeds(3);

    // Assert
    room.TotalBeds.Should().Be(3);
  }

  [Theory]
  [InlineData(0)]
  [InlineData(-1)]
  public void ChangeTotalBeds_ShouldThrowBusinessRuleException_WhenValueIsZeroOrLess(int totalBeds)
  {
    // Arrange
    var room = CreateRoom();

    // Act
    Action act = () => room.ChangeTotalBeds(totalBeds);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void ChangeAvailabilityStatus_ShouldUpdateAvailabilityStatus_WhenValueIsValid()
  {
    // Arrange
    var room = CreateRoom();

    // Act
    room.ChangeAvailabilityStatus(AvailabilityStatus.Occupied);

    // Assert
    room.AvailabilityStatus.Should().Be(AvailabilityStatus.Occupied);
  }

  [Fact]
  public void ChangeAvailabilityStatus_ShouldThrowBusinessRuleException_WhenValueIsInvalid()
  {
    // Arrange
    var room = CreateRoom();
    var invalidAvailabilityStatus = (AvailabilityStatus)999;

    // Act
    Action act = () => room.ChangeAvailabilityStatus(invalidAvailabilityStatus);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void ChangeHygieneStatus_ShouldUpdateHygieneStatus_WhenValueIsValid()
  {
    // Arrange
    var room = CreateRoom();

    // Act
    room.ChangeHygieneStatus(HygieneStatus.Dirty);

    // Assert
    room.HygieneStatus.Should().Be(HygieneStatus.Dirty);
  }

  [Fact]
  public void ChangeHygieneStatus_ShouldThrowBusinessRuleException_WhenValueIsInvalid()
  {
    // Arrange
    var room = CreateRoom();
    var invalidHygieneStatus = (HygieneStatus)999;

    // Act
    Action act = () => room.ChangeHygieneStatus(invalidHygieneStatus);

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
  public void SetAsPendingRestocking_ShouldToggleNeedsRestockingTrue()
  {
    // Arrange
    var room = CreateRoom();

    // Act
    room.SetAsPendingRestocking();

    // Assert
    room.NeedsRestocking.Should().BeTrue();
  }

  [Fact]
  public void SetAsRestocked_ShouldSetNeedsRestockingFalse()
  {
    // Arrange
    var room = CreateRoom();

    // Act
    room.SetAsRestocked();

    // Assert
    room.NeedsRestocking.Should().BeFalse();
  }

  private static Room CreateRoom()
  {
    return new Room(
        Guid.NewGuid(),
        RoomType.Standard,
        "101",
        1,
        2,
        AvailabilityStatus.Available,
        HygieneStatus.Clean);
  }
}
