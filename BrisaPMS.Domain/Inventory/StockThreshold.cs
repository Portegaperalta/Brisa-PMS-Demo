using BrisaPMS.Domain.Shared.Exceptions;

namespace BrisaPMS.Domain.Inventory;

public record StockThreshold
{
    public decimal MinStockThreshold { get; }
    public decimal MaxStockThreshold { get; }

    public StockThreshold(decimal minStockThreshold, decimal maxStockThreshold)
    {
        if (minStockThreshold < 0m)
            throw new BusinessRuleException("Min stock threshold can't be negative");
        
        if (maxStockThreshold < 0m)
            throw new BusinessRuleException("Max stock threshold can't be negative");
        
        MinStockThreshold = minStockThreshold;
        MaxStockThreshold = maxStockThreshold;
    }
}