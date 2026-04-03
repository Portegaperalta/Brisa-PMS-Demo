using BrisaPMS.Domain.Inventory;
using BrisaPMS.Domain.Shared.Exceptions;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Inventory;

public class InventoryMovementTests
{
  [Fact]
  public void Constructor_ShouldCreateInventoryMovement_WhenValuesAreValid()
  {
    // Arrange
    var inventoryItemId = Guid.NewGuid();
    var taskId = Guid.NewGuid();

    // Act
    var result = new InventoryMovement(
        inventoryItemId,
        taskId,
        InventoryMovementType.HousekeepingUsage,
        5m,
        10m,
        5m,
        "Room restocking",
        "Used during housekeeping task");

    // Assert
    result.Id.Should().NotBe(Guid.Empty);
    result.InventoryItemId.Should().Be(inventoryItemId);
    result.TaskId.Should().Be(taskId);
    result.Type.Should().Be(InventoryMovementType.HousekeepingUsage);
    result.Quantity.Should().Be(5m);
    result.QuantityBefore.Should().Be(10m);
    result.QuantityAfter.Should().Be(5m);
    result.Reason.Should().Be("Room restocking");
    result.Notes.Should().Be("Used during housekeeping task");
  }

  [Fact]
  public void Constructor_ShouldAllowEmptyTaskId_WhenValuesAreValid()
  {
    // Arrange
    var inventoryItemId = Guid.NewGuid();

    // Act
    var result = new InventoryMovement(
        inventoryItemId,
        Guid.Empty,
        InventoryMovementType.PurchaseOrder,
        10m,
        0m,
        10m,
        "Initial stock");

    // Assert
    result.TaskId.Should().Be(Guid.Empty);
    result.Notes.Should().BeNull();
  }

  [Fact]
  public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenInventoryItemIdIsEmpty()
  {
    // Arrange
    var inventoryItemId = Guid.Empty;

    // Act
    Action act = () => _ = new InventoryMovement(
        inventoryItemId,
        Guid.NewGuid(),
        InventoryMovementType.HousekeepingUsage,
        5m,
        10m,
        5m,
        "Room restocking");

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenTypeIsInvalid()
  {
    // Arrange
    var invalidType = (InventoryMovementType)999;

    // Act
    Action act = () => _ = new InventoryMovement(
        Guid.NewGuid(),
        Guid.NewGuid(),
        invalidType,
        5m,
        10m,
        5m,
        "Room restocking");

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Theory]
  [InlineData(0)]
  [InlineData(-1)]
  public void Constructor_ShouldThrowBusinessRuleException_WhenQuantityIsLessThanOrEqualToZero(decimal quantity)
  {
    // Act
    Action act = () => _ = new InventoryMovement(
        Guid.NewGuid(),
        Guid.NewGuid(),
        InventoryMovementType.HousekeepingUsage,
        quantity,
        10m,
        5m,
        "Room restocking");

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenQuantityBeforeIsNegative()
  {
    // Arrange
    const decimal quantityBefore = -1m;

    // Act
    Action act = () => _ = new InventoryMovement(
        Guid.NewGuid(),
        Guid.NewGuid(),
        InventoryMovementType.HousekeepingUsage,
        5m,
        quantityBefore,
        5m,
        "Room restocking");

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenQuantityAfterIsNegative()
  {
    // Arrange
    const decimal quantityAfter = -1m;

    // Act
    Action act = () => _ = new InventoryMovement(
        Guid.NewGuid(),
        Guid.NewGuid(),
        InventoryMovementType.HousekeepingUsage,
        5m,
        10m,
        quantityAfter,
        "Room restocking");

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData(" ")]
  public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenReasonIsNullOrWhiteSpace(string? reason)
  {
    // Act
    Action act = () => _ = new InventoryMovement(
        Guid.NewGuid(),
        Guid.NewGuid(),
        InventoryMovementType.HousekeepingUsage,
        5m,
        10m,
        5m,
        reason!);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Fact]
  public void UpdateNotes_ShouldUpdateNotes_WhenValueIsValid()
  {
    // Arrange
    var inventoryMovement = CreateInventoryMovement();

    // Act
    inventoryMovement.UpdateNotes("Adjusted after recount");

    // Assert
    inventoryMovement.Notes.Should().Be("Adjusted after recount");
  }

  [Fact]
  public void UpdateNotes_ShouldAllowEmptyString()
  {
    // Arrange
    var inventoryMovement = CreateInventoryMovement();

    // Act
    inventoryMovement.UpdateNotes(string.Empty);

    // Assert
    inventoryMovement.Notes.Should().BeEmpty();
  }

  private static InventoryMovement CreateInventoryMovement()
  {
    return new InventoryMovement(
        Guid.NewGuid(),
        Guid.NewGuid(),
        InventoryMovementType.HousekeepingUsage,
        5m,
        10m,
        5m,
        "Room restocking",
        "Used during housekeeping task");
  }
}
