using BrisaPMS.Domain.Inventory;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Inventory;

public class InventoryItemTests
{
  [Fact]
  public void Constructor_ShouldCreateInventoryItem_WhenValuesAreValid()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var supplierPhoneNumber = CreatePhoneNumber();
    var supplierEmail = CreateEmail();
    var unitCost = CreateUnitCost();
    var stockThreshold = CreateStockThreshold();

    // Act
    var result = new InventoryItem(
        hotelId,
        "Bath Towels",
        "Large white cotton bath towels",
        "Linen",
        UnitOfMeasure.Piece,
        stockThreshold,
        25m,
        unitCost,
        "Linen Supplier SRL",
        supplierPhoneNumber,
        supplierEmail,
        true);

    // Assert
    result.Id.Should().NotBe(Guid.Empty);
    result.HotelId.Should().Be(hotelId);
    result.Name.Should().Be("Bath Towels");
    result.Description.Should().Be("Large white cotton bath towels");
    result.Category.Should().Be("Linen");
    result.UnitOfMeasure.Should().Be(UnitOfMeasure.Piece);
    result.CurrentStock.Should().Be(0m);
    result.StockThreshold.MinStockThreshold.Should().Be(stockThreshold.MinStockThreshold);
    result.StockThreshold.MaxStockThreshold.Should().Be(stockThreshold.MaxStockThreshold);
    result.ReorderQuantity.Should().Be(25m);
    result.UnitCost.Should().Be(unitCost);
    result.SupplierName.Should().Be("Linen Supplier SRL");
    result.SupplierPhoneNumber.Should().Be(supplierPhoneNumber);
    result.SupplierEmail.Should().Be(supplierEmail);
    result.IsActive.Should().BeTrue();
  }

  [Fact]
  public void Constructor_ShouldCreateInventoryItem_WhenOptionalValuesAreProvided()
  {
    // Arrange
    var hotelId = Guid.NewGuid();

    // Act
    var result = new InventoryItem(
        hotelId,
        "Water Bottles",
        "Complimentary bottled water",
        "Minibar",
        UnitOfMeasure.Box,
        new StockThreshold(5m, 50m),
        10m,
        new Money(2m, CurrencyCode.USD),
        "Refreshments Inc.",
        CreatePhoneNumber(),
        CreateEmail(),
        false,
        20m);

    // Assert
    result.UnitCost.Should().Be(new Money(2m, CurrencyCode.USD));
    result.CurrentStock.Should().Be(20m);
    result.IsActive.Should().BeFalse();
  }

  [Fact]
  public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenHotelIdIsEmpty()
  {
    // Arrange
    var hotelId = Guid.Empty;

    // Act
    Action act = () => _ = new InventoryItem(
        hotelId,
        "Bath Towels",
        "Large white cotton bath towels",
        "Linen",
        UnitOfMeasure.Piece,
        CreateStockThreshold(),
        25m,
        CreateUnitCost(),
        "Linen Supplier SRL",
        CreatePhoneNumber(),
        CreateEmail(),
        true);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenNameIsNullOrEmpty(string? name)
  {
    // Act
    Action act = () => _ = new InventoryItem(
        Guid.NewGuid(),
        name!,
        "Large white cotton bath towels",
        "Linen",
        UnitOfMeasure.Piece,
        CreateStockThreshold(),
        25m,
        CreateUnitCost(),
        "Linen Supplier SRL",
        CreatePhoneNumber(),
        CreateEmail(),
        true);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenDescriptionIsNullOrEmpty(string? description)
  {
    // Act
    Action act = () => _ = new InventoryItem(
        Guid.NewGuid(),
        "Bath Towels",
        description!,
        "Linen",
        UnitOfMeasure.Piece,
        CreateStockThreshold(),
        25m,
        CreateUnitCost(),
        "Linen Supplier SRL",
        CreatePhoneNumber(),
        CreateEmail(),
        true);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenCategoryIsNullOrEmpty(string? category)
  {
    // Act
    Action act = () => _ = new InventoryItem(
        Guid.NewGuid(),
        "Bath Towels",
        "Large white cotton bath towels",
        category!,
        UnitOfMeasure.Piece,
        CreateStockThreshold(),
        25m,
        CreateUnitCost(),
        "Linen Supplier SRL",
        CreatePhoneNumber(),
        CreateEmail(),
        true);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenSupplierNameIsNullOrEmpty(string? supplierName)
  {
    // Act
    Action act = () => _ = new InventoryItem(
        Guid.NewGuid(),
        "Bath Towels",
        "Large white cotton bath towels",
        "Linen",
        UnitOfMeasure.Piece,
        CreateStockThreshold(),
        25m,
        CreateUnitCost(),
        supplierName!,
        CreatePhoneNumber(),
        CreateEmail(),
        true);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenUnitOfMeasureIsInvalid()
  {
    // Arrange
    var invalidUnitOfMeasure = (UnitOfMeasure)999;

    // Act
    Action act = () => _ = new InventoryItem(
        Guid.NewGuid(),
        "Bath Towels",
        "Large white cotton bath towels",
        "Linen",
        invalidUnitOfMeasure,
        CreateStockThreshold(),
        25m,
        CreateUnitCost(),
        "Linen Supplier SRL",
        CreatePhoneNumber(),
        CreateEmail(),
        true);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenCurrentStockIsNegative()
  {
    // Act
    Action act = () => _ = new InventoryItem(
        Guid.NewGuid(),
        "Bath Towels",
        "Large white cotton bath towels",
        "Linen",
        UnitOfMeasure.Piece,
        CreateStockThreshold(),
        25m,
        CreateUnitCost(),
        "Linen Supplier SRL",
        CreatePhoneNumber(),
        CreateEmail(),
        true,
        -1m);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenReorderQuantityIsNegative()
  {
    // Act
    Action act = () => _ = new InventoryItem(
        Guid.NewGuid(),
        "Bath Towels",
        "Large white cotton bath towels",
        "Linen",
        UnitOfMeasure.Piece,
        CreateStockThreshold(),
        -1m,
        CreateUnitCost(),
        "Linen Supplier SRL",
        CreatePhoneNumber(),
        CreateEmail(),
        true);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenMinStockThresholdIsNegative()
  {
    // Act
    Action act = () => _ = new StockThreshold(-1m, 100m);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenMaxStockThresholdIsNegative()
  {
    // Act
    Action act = () => _ = new StockThreshold(10m, -1m);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void UpdateName_ShouldUpdateName_WhenValueIsValid()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem();

    // Act
    inventoryItem.UpdateName("Hand Towels");

    // Assert
    inventoryItem.Name.Should().Be("Hand Towels");
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  public void UpdateName_ShouldThrowEmptyRequiredFieldException_WhenValueIsNullOrEmpty(string? newName)
  {
    // Arrange
    var inventoryItem = CreateInventoryItem();

    // Act
    Action act = () => inventoryItem.UpdateName(newName!);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Fact]
  public void UpdateDescription_ShouldUpdateDescription_WhenValueIsValid()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem();

    // Act
    inventoryItem.UpdateDescription("Soft hand towels for guest bathrooms");

    // Assert
    inventoryItem.Description.Should().Be("Soft hand towels for guest bathrooms");
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  public void UpdateDescription_ShouldThrowEmptyRequiredFieldException_WhenValueIsNullOrEmpty(string? newDescription)
  {
    // Arrange
    var inventoryItem = CreateInventoryItem();

    // Act
    Action act = () => inventoryItem.UpdateDescription(newDescription!);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Fact]
  public void ChangeCategory_ShouldUpdateCategory_WhenValueIsValid()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem();

    // Act
    inventoryItem.ChangeCategory("Bathroom Supplies");

    // Assert
    inventoryItem.Category.Should().Be("Bathroom Supplies");
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  public void ChangeCategory_ShouldThrowEmptyRequiredFieldException_WhenValueIsNullOrEmpty(string? newCategory)
  {
    // Arrange
    var inventoryItem = CreateInventoryItem();

    // Act
    Action act = () => inventoryItem.ChangeCategory(newCategory!);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Fact]
  public void ChangeUnitOfMeasure_ShouldUpdateUnitOfMeasure_WhenValueIsValid()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem();

    // Act
    inventoryItem.ChangeUnitOfMeasure(UnitOfMeasure.Pack);

    // Assert
    inventoryItem.UnitOfMeasure.Should().Be(UnitOfMeasure.Pack);
  }

  [Fact]
  public void ChangeUnitOfMeasure_ShouldThrowBusinessRuleException_WhenValueIsInvalid()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem();
    var invalidUnitOfMeasure = (UnitOfMeasure)999;

    // Act
    Action act = () => inventoryItem.ChangeUnitOfMeasure(invalidUnitOfMeasure);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void IncreaseCurrentStock_ShouldIncreaseCurrentStock_WhenItemIsActive()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem(currentStock: 10m);

    // Act
    inventoryItem.IncreaseCurrentStock(5m);

    // Assert
    inventoryItem.CurrentStock.Should().Be(15m);
  }

  [Fact]
  public void IncreaseCurrentStock_ShouldThrowBusinessRuleException_WhenItemIsInactive()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem(isActive: false, currentStock: 10m);

    // Act
    Action act = () => inventoryItem.IncreaseCurrentStock(5m);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void IncreaseCurrentStock_ShouldThrowBusinessRuleException_WhenCurrentStockAlreadyExceedsMaxThreshold()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem(currentStock: 101m, maxStockThreshold: 100m);

    // Act
    Action act = () => inventoryItem.IncreaseCurrentStock(1m);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void DecreaseCurrentStock_ShouldDecreaseCurrentStock_WhenItemIsActiveAndEnoughStockIsAvailable()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem(currentStock: 10m);

    // Act
    inventoryItem.DecreaseCurrentStock(4m);

    // Assert
    inventoryItem.CurrentStock.Should().Be(6m);
  }

  [Fact]
  public void DecreaseCurrentStock_ShouldThrowBusinessRuleException_WhenItemIsInactive()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem(isActive: false, currentStock: 10m);

    // Act
    Action act = () => inventoryItem.DecreaseCurrentStock(1m);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void DecreaseCurrentStock_ShouldThrowBusinessRuleException_WhenRequestedAmountIsHigherThanCurrentStock()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem(currentStock: 5m);

    // Act
    Action act = () => inventoryItem.DecreaseCurrentStock(6m);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void DecreaseCurrentStock_ShouldThrowBusinessRuleException_WhenThereIsNoStockAvailable()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem(currentStock: 0m);

    // Act
    Action act = () => inventoryItem.DecreaseCurrentStock(0m);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void UpdateReorderQuantity_ShouldUpdateReorderQuantity_WhenValueIsValid()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem();

    // Act
    inventoryItem.UpdateReorderQuantity(40m);

    // Assert
    inventoryItem.ReorderQuantity.Should().Be(40m);
  }

  [Fact]
  public void UpdateReorderQuantity_ShouldThrowBusinessRuleException_WhenValueIsNegative()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem();

    // Act
    Action act = () => inventoryItem.UpdateReorderQuantity(-1m);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void UpdateStockThreshold_ShouldUpdateStockThreshold_WhenValueIsValid()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem();
    var newStockThreshold = new StockThreshold(20m, 120m);

    // Act
    inventoryItem.UpdateStockThreshold(newStockThreshold);

    // Assert
    inventoryItem.StockThreshold.Should().Be(newStockThreshold);
  }

  [Fact]
  public void UpdateUnitCost_ShouldUpdateUnitCost_WhenValueIsValid()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem();
    var newUnitCost = new Money(10m, CurrencyCode.USD);

    // Act
    inventoryItem.UpdateUnitCost(newUnitCost);

    // Assert
    inventoryItem.UnitCost.Should().Be(newUnitCost);
  }

  [Fact]
  public void UpdateSupplierName_ShouldUpdateSupplierName_WhenValueIsValid()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem();

    // Act
    inventoryItem.UpdateSupplierName("Hotel Essentials Co.");

    // Assert
    inventoryItem.SupplierName.Should().Be("Hotel Essentials Co.");
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  public void UpdateSupplierName_ShouldThrowEmptyRequiredFieldException_WhenValueIsNullOrEmpty(string? newSupplierName)
  {
    // Arrange
    var inventoryItem = CreateInventoryItem();

    // Act
    Action act = () => inventoryItem.UpdateSupplierName(newSupplierName!);

    // Assert
    act.Should().Throw<EmptyRequiredFieldException>();
  }

  [Fact]
  public void UpdateSupplierPhoneNumber_ShouldUpdateSupplierPhoneNumber_WhenValueIsValid()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem();
    var newSupplierPhoneNumber = new PhoneNumber("+1 829 555 9876");

    // Act
    inventoryItem.UpdateSupplierPhoneNumber(newSupplierPhoneNumber);

    // Assert
    inventoryItem.SupplierPhoneNumber.Should().Be(newSupplierPhoneNumber);
  }

  [Fact]
  public void UpdateSupplierEmail_ShouldUpdateSupplierEmail_WhenValueIsValid()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem();
    var newSupplierEmail = new Email("supplies@hotelessentials.com");

    // Act
    inventoryItem.UpdateSupplierEmail(newSupplierEmail);

    // Assert
    inventoryItem.SupplierEmail.Should().Be(newSupplierEmail);
  }

  [Fact]
  public void SetAsActive_ShouldSetIsActiveToTrue()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem(isActive: false);

    // Act
    inventoryItem.SetAsActive();

    // Assert
    inventoryItem.IsActive.Should().BeTrue();
  }

  [Fact]
  public void SetAsInactive_ShouldSetIsActiveToFalse()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem(isActive: true);

    // Act
    inventoryItem.SetAsInactive();

    // Assert
    inventoryItem.IsActive.Should().BeFalse();
  }

  private static InventoryItem CreateInventoryItem(
      bool isActive = true,
      decimal currentStock = 10m,
      decimal maxStockThreshold = 100m)
  {
    return new InventoryItem(
        Guid.NewGuid(),
        "Bath Towels",
        "Large white cotton bath towels",
        "Linen",
        UnitOfMeasure.Piece,
        CreateStockThreshold(maxStockThreshold: maxStockThreshold),
        25m,
        CreateUnitCost(),
        "Linen Supplier SRL",
        CreatePhoneNumber(),
        CreateEmail(),
        isActive,
        currentStock);
  }

  private static PhoneNumber CreatePhoneNumber()
  {
    return new PhoneNumber("+1 809 555 1234");
  }

  private static Email CreateEmail()
  {
    return new Email("supplier@linen.com");
  }

  private static Money CreateUnitCost()
  {
    return new Money(8.5m, CurrencyCode.DOP);
  }

  private static StockThreshold CreateStockThreshold(
      decimal minStockThreshold = 10m,
      decimal maxStockThreshold = 100m)
  {
    return new StockThreshold(minStockThreshold, maxStockThreshold);
  }
}
