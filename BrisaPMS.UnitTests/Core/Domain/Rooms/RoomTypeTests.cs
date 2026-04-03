using BrisaPMS.Domain.Rooms;
using BrisaPMS.Domain.Shared.Exceptions;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Rooms;

public class RoomTypeTests
{
  [Fact]
  public void Constructor_ShouldCreateRoomType_WhenValuesAreValid()
  {
    // Arrange
    const string name = "Deluxe Suite";
    const string description = "Spacious suite with ocean view";

    // Act
    var result = new RoomType(name, 250m, 2, BedType.Queen, 2, 1, description);

    // Assert
    result.Id.Should().NotBe(Guid.Empty);
    result.Name.Should().Be(name);
    result.Description.Should().Be(description);
    result.BaseRate.Should().Be(250m);
    result.TotalBeds.Should().Be(2);
    result.BedType.Should().Be(BedType.Queen);
    result.MaxOccupancyAdults.Should().Be(2);
    result.MaxOccupancyChildren.Should().Be(1);
  }

  [Fact]
  public void Constructor_ShouldCreateRoomType_WhenDescriptionIsNotProvided()
  {
    // Arrange
    // Act
    var result = new RoomType("Standard Room", 120m, 1, BedType.Double, 2, 0);

    // Assert
    result.Description.Should().BeNull();
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData(" ")]
  public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenNameIsNullOrWhiteSpace(string? name)
  {
    // Act
    Action act = () => _ = new RoomType(name!, 120m, 1, BedType.Double, 2, 0);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenBaseRateIsNegative()
  {
    // Arrange
    const decimal baseRate = -1m;

    // Act
    Action act = () => _ = new RoomType("Standard Room", baseRate, 1, BedType.Double, 2, 0);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenTotalBedsIsZeroOrLess()
  {
    // Arrange
    const int totalBeds = 0;

    // Act
    Action act = () => _ = new RoomType("Standard Room", 120m, totalBeds, BedType.Double, 2, 0);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenBedTypeIsInvalid()
  {
    // Arrange
    var invalidBedType = (BedType)999;

    // Act
    Action act = () => _ = new RoomType("Standard Room", 120m, 1, invalidBedType, 2, 0);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenMaxOccupancyAdultsIsZeroOrLess()
  {
    // Arrange
    const int maxOccupancyAdults = 0;

    // Act
    Action act = () => _ = new RoomType("Standard Room", 120m, 1, BedType.Double, maxOccupancyAdults, 0);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenMaxOccupancyChildrenIsNegative()
  {
    // Arrange
    const int maxOccupancyChildren = -1;

    // Act
    Action act = () => _ = new RoomType("Standard Room", 120m, 1, BedType.Double, 2, maxOccupancyChildren);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void UpdateName_ShouldUpdateName_WhenValueIsValid()
  {
    // Arrange
    var roomType = CreateRoomType();

    // Act
    roomType.UpdateName("Family Suite");

    // Assert
    roomType.Name.Should().Be("Family Suite");
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData(" ")]
  public void UpdateName_ShouldThrowEmptyRequiredFieldException_WhenValueIsNullOrWhiteSpace(string? newName)
  {
    // Arrange
    var roomType = CreateRoomType();

    // Act
    Action act = () => roomType.UpdateName(newName!);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Fact]
  public void UpdateDescription_ShouldUpdateDescription_WhenValueIsValid()
  {
    // Arrange
    var roomType = CreateRoomType();

    // Act
    roomType.UpdateDescription("Updated description");

    // Assert
    roomType.Description.Should().Be("Updated description");
  }

  [Fact]
  public void UpdateBaseRate_ShouldUpdateBaseRate_WhenValueIsValid()
  {
    // Arrange
    var roomType = CreateRoomType();

    // Act
    roomType.UpdateBaseRate(300m);

    // Assert
    roomType.BaseRate.Should().Be(300m);
  }

  [Fact]
  public void UpdateBaseRate_ShouldThrowBusinessRuleException_WhenValueIsNegative()
  {
    // Arrange
    var roomType = CreateRoomType();

    // Act
    Action act = () => roomType.UpdateBaseRate(-1m);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void UpdateTotalBeds_ShouldUpdateTotalBeds_WhenValueIsValid()
  {
    // Arrange
    var roomType = CreateRoomType();

    // Act
    roomType.UpdateTotalBeds(3);

    // Assert
    roomType.TotalBeds.Should().Be(3);
  }

  [Fact]
  public void UpdateTotalBeds_ShouldThrowBusinessRuleException_WhenValueIsZeroOrLess()
  {
    // Arrange
    var roomType = CreateRoomType();

    // Act
    Action act = () => roomType.UpdateTotalBeds(0);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void UpdateBedType_ShouldUpdateBedType_WhenValueIsValid()
  {
    // Arrange
    var roomType = CreateRoomType();

    // Act
    roomType.UpdateBedType(BedType.King);

    // Assert
    roomType.BedType.Should().Be(BedType.King);
  }

  [Fact]
  public void UpdateBedType_ShouldThrowBusinessRuleException_WhenValueIsInvalid()
  {
    // Arrange
    var roomType = CreateRoomType();
    var invalidBedType = (BedType)999;

    // Act
    Action act = () => roomType.UpdateBedType(invalidBedType);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void UpdateMaxOccupancyAdults_ShouldUpdateMaxOccupancyAdults_WhenValueIsValid()
  {
    // Arrange
    var roomType = CreateRoomType();

    // Act
    roomType.UpdateMaxOccupancyAdults(4);

    // Assert
    roomType.MaxOccupancyAdults.Should().Be(4);
  }

  [Fact]
  public void UpdateMaxOccupancyAdults_ShouldThrowBusinessRuleException_WhenValueIsZeroOrLess()
  {
    // Arrange
    var roomType = CreateRoomType();

    // Act
    Action act = () => roomType.UpdateMaxOccupancyAdults(0);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void UpdateMaxOccupancyChildren_ShouldUpdateMaxOccupancyChildren_WhenValueIsValid()
  {
    // Arrange
    var roomType = CreateRoomType();

    // Act
    roomType.UpdateMaxOccupancyChildren(2);

    // Assert
    roomType.MaxOccupancyChildren.Should().Be(2);
  }

  [Fact]
  public void UpdateMaxOccupancyChildren_ShouldThrowBusinessRuleException_WhenValueIsNegative()
  {
    // Arrange
    var roomType = CreateRoomType();

    // Act
    Action act = () => roomType.UpdateMaxOccupancyChildren(-1);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  private static RoomType CreateRoomType()
  {
    return new RoomType(
        "Deluxe Suite",
        250m,
        2,
        BedType.Queen,
        2,
        1,
        "Spacious suite with ocean view");
  }
}
