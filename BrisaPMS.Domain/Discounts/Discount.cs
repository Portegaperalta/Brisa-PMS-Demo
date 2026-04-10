using BrisaPMS.Domain.Discount;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Domain.Discounts;

public class Discount
{
    public Guid Id { get; init; }
    public Guid HotelId { get; init; }
    public string Name { get; private set; }
    public string Type { get; private set; }
    public Money Value { get;  private set; }
    public DiscountTimeInterval TimeInterval { get; private set; }
    public bool IsActive { get; private set; }

    public Discount
    (
        Guid hotelId,
        string name,
        string type,
        Money value,
        DiscountTimeInterval timeInterval,
        bool isActive
    )
    {
        if (hotelId == Guid.Empty)
            throw new EmptyRequiredFieldException("Hotel Id");
        
        if (string.IsNullOrEmpty(name))
            throw new EmptyRequiredFieldException("Discount Name");
        
        if (string.IsNullOrEmpty(type))
            throw new EmptyRequiredFieldException("Discount Type");
        
        Id = Guid.CreateVersion7();
        HotelId = hotelId;
        Name = name;
        Type = type;
        Value = value;
        TimeInterval = timeInterval;
        IsActive = isActive;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrEmpty(newName))
            throw new EmptyRequiredFieldException("Discount Name");
        
        Name = newName;
    }

    public void UpdateType(string newType)
    {
        if (string.IsNullOrEmpty(newType))
            throw new EmptyRequiredFieldException("Discount Type");
        
        Type = newType;
    }

    public void UpdateValue(Money newValue) => Value = newValue;

    public void UpdateTimeInterval(DiscountTimeInterval newTimeInterval)
    {
        if (IsActive is not true)
            throw new BusinessRuleException("Discount not active, unable to update time interval");
        
        TimeInterval = newTimeInterval;
    }
    
    public void SetAsActive() => IsActive = true;
    
    public void SetAsInactive() => IsActive = false;
}