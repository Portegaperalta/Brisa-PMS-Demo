using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.RoomTypes;
using BrisaPMS.Domain.Shared.Exceptions;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.RoomTypes;

public class RoomTypeTests
{
  [Fact]
  public void Constructor_ShouldCreateRoomType_WhenValuesAreValid()
  {
    // Arrange
    const string name = "Deluxe Suite";
    const string description = "Spacious suite with ocean view";
    var occupancyPolicy = new OccupancyPolicy(2, 1);
    var beds = new RoomBed(BedType.Queen, 2);
    var baseRate = new RoomBaseRate(25m);

    // Act
    var result = new RoomType(name, baseRate, beds, occupancyPolicy, description);

    // Assert
    result.Id.Should().NotBe(Guid.Empty);
    result.Name.Should().Be(name);
    result.Description.Should().Be(description);
    result.BaseRate.Should().Be(baseRate);
    result.Beds.NumberOfBeds.Should().Be(2);
    result.Beds.BedType.Should().Be(BedType.Queen);
    result.OccupancyPolicy.MaxOccupancyAdults.Should().Be(2);
    result.OccupancyPolicy.MaxOccupancyChildren.Should().Be(1);
  }

  [Fact]
  public void Constructor_ShouldCreateRoomType_WhenDescriptionIsNotProvided()
  {
    // Arrange
    var beds = new RoomBed(BedType.Double, 1);

    // Act
    var result = new RoomType("Standard Room", CreateBaseRate(), beds, CreateOccupancyPolicy());

    // Assert
    result.Description.Should().BeNull();
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData(" ")]
  public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenNameIsNullOrWhiteSpace(string? name)
  {
    // Arrange
    var beds = new RoomBed(BedType.Double, 1);

    // Act
    Action act = () => _ = new RoomType(name!, CreateBaseRate(), beds, CreateOccupancyPolicy());

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenBedTypeIsInvalid()
  {
    // Act
    Action act = () => _ = new RoomBed((BedType)999, 1);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenMaxOccupancyAdultsIsZeroOrLess()
  {
    // Arrange
    const int maxOccupancyAdults = 0;

    // Act
    Action act = () => _ = new OccupancyPolicy(maxOccupancyAdults, 0);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenMaxOccupancyChildrenIsNegative()
  {
    // Arrange
    const int maxOccupancyChildren = -1;

    // Act
    Action act = () => _ = new OccupancyPolicy(2, maxOccupancyChildren);

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
    var newBaseRate = CreateBaseRate();
    
    // Act
    roomType.UpdateBaseRate(newBaseRate);

    // Assert
    roomType.BaseRate.Should().Be(newBaseRate);
  }

  [Fact]
  public void UpdateRoomBeds_ShouldUpdateRoomBeds_WhenValueIsValid()
  {
    // Arrange
    var roomType = CreateRoomType();
    var newBeds = new RoomBed(BedType.King, 3);

    // Act
    roomType.UpdateRoomBeds(newBeds);

    // Assert
    roomType.Beds.NumberOfBeds.Should().Be(3);
    roomType.Beds.BedType.Should().Be(BedType.King);
  }

  [Fact]
  public void UpdateRoomBeds_ShouldThrowBusinessRuleException_WhenNumberOfBedsIsZeroOrLess()
  {
    // Arrange
    var roomType = CreateRoomType();

    // Act
    Action act = () => roomType.UpdateRoomBeds(new RoomBed(BedType.King, 0));

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void UpdateRoomBeds_ShouldThrowBusinessRuleException_WhenBedTypeIsInvalid()
  {
    // Arrange
    var roomType = CreateRoomType();

    // Act
    Action act = () => roomType.UpdateRoomBeds(new RoomBed((BedType)999, 1));

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void UpdateOccupancyPolicy_ShouldUpdateOccupancyPolicy_WhenValueIsValid()
  {
    // Arrange
    var roomType = CreateRoomType();
    var newOccupancyPolicy = new OccupancyPolicy(4, 2);

    // Act
    roomType.UpdateOccupancyPolicy(newOccupancyPolicy);

    // Assert
    roomType.OccupancyPolicy.MaxOccupancyAdults.Should().Be(4);
    roomType.OccupancyPolicy.MaxOccupancyChildren.Should().Be(2);
  }

  [Fact]
  public void UpdateOccupancyPolicy_ShouldThrowBusinessRuleException_WhenMaxOccupancyAdultsIsZeroOrLess()
  {
    // Arrange
    var roomType = CreateRoomType();

    // Act
    Action act = () => roomType.UpdateOccupancyPolicy(new OccupancyPolicy(0, 0));

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void UpdateOccupancyPolicy_ShouldThrowBusinessRuleException_WhenMaxOccupancyChildrenIsNegative()
  {
    // Arrange
    var roomType = CreateRoomType();

    // Act
    Action act = () => roomType.UpdateOccupancyPolicy(new OccupancyPolicy(2, -1));

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  private static RoomType CreateRoomType()
  {
    return new RoomType(
        "Deluxe Suite",
        new RoomBaseRate(0.5m),
        new RoomBed(BedType.Queen, 2),
        CreateOccupancyPolicy(),
        "Spacious suite with ocean view");
  }

  private static OccupancyPolicy CreateOccupancyPolicy() => new OccupancyPolicy(2, 1);

  private static RoomBaseRate CreateBaseRate() => new RoomBaseRate(0.10m);
}