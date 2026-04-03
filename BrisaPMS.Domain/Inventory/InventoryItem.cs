using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Domain.Inventory;

public class InventoryItem
{
    public Guid Id { get; init; }
    public Guid HotelId { get; init; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Category { get; private set; }
    public UnitOfMeasure UnitOfMeasure { get; private set; }
    public decimal CurrentStock { get;  private set; }
    public decimal MinStockThreshold { get; private set; }
    public decimal MaxStockThreshold { get; private set; }
    public decimal ReorderQuantity { get; private set; }
    public decimal UnitCost { get; private set; }
    public CurrencyCode CurrencyCode { get; private set; }
    public string SupplierName { get; private set; }
    public PhoneNumber SupplierPhoneNumber { get; private set; }
    public Email SupplierEmail { get; private set; }
    public bool IsActive { get; private set; }

    public InventoryItem
    (
        Guid hotelId,
        string name,
        string description,
        string category,
        UnitOfMeasure unitOfMeasure,
        decimal minStockThreshold,
        decimal maxStockThreshold,
        decimal reorderQuantity,
        decimal unitCost,
        string supplierName,
        PhoneNumber supplierPhoneNumber,
        Email supplierEmail,
        bool isActive,
        CurrencyCode currencyCode = CurrencyCode.DOP,
        decimal currentStock = 0m
    )
    {
        if (hotelId ==  Guid.Empty)
            throw new EmptyRequiredFieldException("Hotel Id");
        
        if (string.IsNullOrEmpty(name))
            throw new EmptyRequiredFieldException("Name");
        
        if (string.IsNullOrEmpty(description))
            throw new EmptyRequiredFieldException("Description");
        
        if (string.IsNullOrEmpty(category))
            throw new EmptyRequiredFieldException("Category");
        
        if (Enum.IsDefined<UnitOfMeasure>(unitOfMeasure) is false)
            throw new BusinessRuleException("Unit of measure not supported");
        
        if (currentStock < 0m)
            throw new BusinessRuleException("Current stock can't be negative");
        
        if (minStockThreshold < 0m)
            throw new BusinessRuleException("Min stock threshold can't be negative");
        
        if (maxStockThreshold < 0m)
            throw new BusinessRuleException("Max stock threshold can't be negative");
        
        if (reorderQuantity < 0m)
            throw new BusinessRuleException("Reorder quantity can't be negative");
        
        if (unitCost < 0m)
            throw new BusinessRuleException("Unit cost can't be negative");
        
        if (Enum.IsDefined<CurrencyCode>(currencyCode) is false)
            throw new BusinessRuleException("Currency code not supported");
        
        if  (string.IsNullOrEmpty(supplierName))
            throw new EmptyRequiredFieldException("Supplier Name");

        Id = Guid.CreateVersion7();
        HotelId = hotelId;
        Name = name;
        Description = description;
        Category = category;
        UnitOfMeasure = unitOfMeasure;
        CurrentStock = currentStock;
        MinStockThreshold = minStockThreshold;
        MaxStockThreshold = maxStockThreshold;
        ReorderQuantity = reorderQuantity;
        UnitCost = unitCost;
        CurrencyCode = currencyCode;
        SupplierName = supplierName;
        SupplierPhoneNumber = supplierPhoneNumber;
        SupplierEmail = supplierEmail;
        IsActive = isActive;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrEmpty(newName))
            throw new EmptyRequiredFieldException("Name");
        
        Name = newName;
    }

    public void UpdateDescription(string newDescription)
    {
        if (string.IsNullOrEmpty(newDescription))
            throw new EmptyRequiredFieldException("Description");
        
        Description = newDescription;
    }

    public void ChangeCategory(string newCategory)
    {
        if (string.IsNullOrEmpty(newCategory))
            throw new EmptyRequiredFieldException("Category");
        
        Category = newCategory;
    }

    public void ChangeUnitOfMeasure(UnitOfMeasure newUnitOfMeasure)
    {
        if (Enum.IsDefined<UnitOfMeasure>(newUnitOfMeasure) is false)
            throw new BusinessRuleException("Unit of measure not supported");
        
        UnitOfMeasure = newUnitOfMeasure;
    }

    public void IncreaseCurrentStock(decimal newStockAmount)
    {
        if (IsActive is false)
            throw new BusinessRuleException("Item is not active,  can't increase stock");
        
        if (CurrentStock > MaxStockThreshold)
            throw new BusinessRuleException("Max stock threshold exceeded, can't increase current stock");
        
        CurrentStock += newStockAmount;
    }

    public void DecreaseCurrentStock(decimal stockTakenAmount)
    {
        if (IsActive is false)
            throw new BusinessRuleException("Item is not active,  can't decrease stock");
        
        if (stockTakenAmount > CurrentStock)
            throw new BusinessRuleException("Retrieved stock amount can't be higher than current stock");
        
        if (CurrentStock == 0)
            throw new BusinessRuleException("No more stock available, can't decrease current stock");
        
        CurrentStock -= stockTakenAmount;
    }

    public void UpdateReorderQuantity(decimal newReorderQuantity)
    {
        if (newReorderQuantity < 0m)
            throw new BusinessRuleException("Reorder quantity can't be negative");
        
        ReorderQuantity = newReorderQuantity;
    }

    public void UpdateUnitCost(decimal newUnitCost)
    {
        if (newUnitCost < 0m)
            throw new BusinessRuleException("Unit cost can't be negative");
        
        UnitCost = newUnitCost;
    }

    public void ChangeCurrencyCode(CurrencyCode newCurrencyCode)
    {
        if (Enum.IsDefined<CurrencyCode>(newCurrencyCode) is false)
            throw new BusinessRuleException("Currency code not supported");
        
        CurrencyCode = newCurrencyCode;
    }

    public void UpdateSupplierName(string newSupplierName)
    {
        if (string.IsNullOrEmpty(newSupplierName))
            throw new EmptyRequiredFieldException("Supplier Name");
        
        SupplierName = newSupplierName;
    }
    
    public void UpdateSupplierPhoneNumber(PhoneNumber newSupplierPhoneNumber) 
        => SupplierPhoneNumber = newSupplierPhoneNumber;
    
    public void UpdateSupplierEmail(Email newSupplierEmail)
        => SupplierEmail = newSupplierEmail;
    
    public void SetAsActive() => IsActive = true;
    
    public void SetAsInactive() => IsActive = false;
}