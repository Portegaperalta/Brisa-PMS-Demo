using BrisaPMS.Domain.Entities;
using BrisaPMS.Domain.Enums;
using BrisaPMS.Domain.Exceptions;
using BrisaPMS.Domain.ValueObjects;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Entities;

public class InventoryItemTests
{
  [Fact]
  public void Constructor_ShouldCreateInventoryItem_WhenValuesAreValid()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var supplierPhoneNumber = CreatePhoneNumber();
    var supplierEmail = CreateEmail();

    // Act
    var result = new InventoryItem(
        hotelId,
        "Bath Towels",
        "Large white cotton bath towels",
        "Linen",
        UnitOfMeasure.Piece,
        10m,
        100m,
        25m,
        8.5m,
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
    result.MinStockThreshold.Should().Be(10m);
    result.MaxStockThreshold.Should().Be(100m);
    result.ReorderQuantity.Should().Be(25m);
    result.UnitCost.Should().Be(8.5m);
    result.CurrencyCode.Should().Be(CurrencyCode.DOP);
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
        5m,
        50m,
        10m,
        2m,
        "Refreshments Inc.",
        CreatePhoneNumber(),
        CreateEmail(),
        false,
        CurrencyCode.USD,
        20m);

    // Assert
    result.CurrencyCode.Should().Be(CurrencyCode.USD);
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
        10m,
        100m,
        25m,
        8.5m,
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
        10m,
        100m,
        25m,
        8.5m,
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
        10m,
        100m,
        25m,
        8.5m,
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
        10m,
        100m,
        25m,
        8.5m,
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
        10m,
        100m,
        25m,
        8.5m,
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
        10m,
        100m,
        25m,
        8.5m,
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
        10m,
        100m,
        25m,
        8.5m,
        "Linen Supplier SRL",
        CreatePhoneNumber(),
        CreateEmail(),
        true,
        CurrencyCode.DOP,
        -1m);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Theory]
  [InlineData(-1, 100, 25, 8.5)]
  [InlineData(10, -1, 25, 8.5)]
  [InlineData(10, 100, -1, 8.5)]
  [InlineData(10, 100, 25, -1)]
  public void Constructor_ShouldThrowBusinessRuleException_WhenNumericValuesAreNegative(
      decimal minStockThreshold,
      decimal maxStockThreshold,
      decimal reorderQuantity,
      decimal unitCost)
  {
    // Act
    Action act = () => _ = new InventoryItem(
        Guid.NewGuid(),
        "Bath Towels",
        "Large white cotton bath towels",
        "Linen",
        UnitOfMeasure.Piece,
        minStockThreshold,
        maxStockThreshold,
        reorderQuantity,
        unitCost,
        "Linen Supplier SRL",
        CreatePhoneNumber(),
        CreateEmail(),
        true);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void Constructor_ShouldThrowBusinessRuleException_WhenCurrencyCodeIsInvalid()
  {
    // Arrange
    var invalidCurrencyCode = (CurrencyCode)999;

    // Act
    Action act = () => _ = new InventoryItem(
        Guid.NewGuid(),
        "Bath Towels",
        "Large white cotton bath towels",
        "Linen",
        UnitOfMeasure.Piece,
        10m,
        100m,
        25m,
        8.5m,
        "Linen Supplier SRL",
        CreatePhoneNumber(),
        CreateEmail(),
        true,
        invalidCurrencyCode);

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
  public void UpdateUnitCost_ShouldUpdateUnitCost_WhenValueIsValid()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem();

    // Act
    inventoryItem.UpdateUnitCost(10m);

    // Assert
    inventoryItem.UnitCost.Should().Be(10m);
  }

  [Fact]
  public void UpdateUnitCost_ShouldThrowBusinessRuleException_WhenValueIsNegative()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem();

    // Act
    Action act = () => inventoryItem.UpdateUnitCost(-1m);

    // Assert
    act.Should().Throw<BusinessRuleException>();
  }

  [Fact]
  public void ChangeCurrencyCode_ShouldUpdateCurrencyCode_WhenValueIsValid()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem();

    // Act
    inventoryItem.ChangeCurrencyCode(CurrencyCode.USD);

    // Assert
    inventoryItem.CurrencyCode.Should().Be(CurrencyCode.USD);
  }

  [Fact]
  public void ChangeCurrencyCode_ShouldThrowBusinessRuleException_WhenValueIsInvalid()
  {
    // Arrange
    var inventoryItem = CreateInventoryItem();
    var invalidCurrencyCode = (CurrencyCode)999;

    // Act
    Action act = () => inventoryItem.ChangeCurrencyCode(invalidCurrencyCode);

    // Assert
    act.Should().Throw<BusinessRuleException>();
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
        10m,
        maxStockThreshold,
        25m,
        8.5m,
        "Linen Supplier SRL",
        CreatePhoneNumber(),
        CreateEmail(),
        isActive,
        CurrencyCode.DOP,
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
}
