using BrisaPMS.Domain.Enums;
using BrisaPMS.Domain.Exceptions;

namespace BrisaPMS.Domain.Entities;

public class InventoryMovement
{
    public Guid Id { get; init; }
    public Guid InventoryItemId { get; init; }
    public Guid? TaskId {get; init; }
    public InventoryMovementType Type { get; init; }
    public decimal Quantity { get; init; }
    public decimal QuantityBefore { get; init; }
    public decimal QuantityAfter { get; init; }
    public string Reason { get; init; }
    public string? Notes { get; private set; }

    public InventoryMovement
    (
        Guid inventoryItemId,
        Guid taskId,
        InventoryMovementType type,
        decimal quantity,
        decimal quantityBefore,
        decimal quantityAfter,
        string reason,
        string? notes = null
    )
    {
        if (inventoryItemId == Guid.Empty)
            throw new EmptyRequiredFieldException("Inventory Item Id");
        
        if (Enum.IsDefined<InventoryMovementType>(type) is not true)
            throw new BusinessRuleException("Inventory movement not supported");
        
        if (quantity <= 0m)
            throw new BusinessRuleException("Quantity taken can't be less or equal than zero");
        
        if  (quantityBefore < 0m)
            throw new BusinessRuleException("Quantity before can't be less than zero");
        
        if (quantityAfter < 0m)
            throw new BusinessRuleException("Quantity after can't be less than zero");
        
        if (string.IsNullOrWhiteSpace(reason))
            throw new EmptyRequiredFieldException("Movement reason");

        Id = Guid.CreateVersion7();
        InventoryItemId = inventoryItemId;
        TaskId = taskId;
        Type = type;
        Quantity = quantity;
        QuantityBefore = quantityBefore;
        QuantityAfter = quantityAfter;
        Reason = reason;
        Notes = notes;
    }
    
    public void UpdateNotes (string newNotes) =>  Notes = newNotes;
}