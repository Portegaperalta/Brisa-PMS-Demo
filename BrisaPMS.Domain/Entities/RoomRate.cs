using BrisaPMS.Domain.Enums;
using BrisaPMS.Domain.Exceptions;
using BrisaPMS.Domain.ValueObjects;

namespace BrisaPMS.Domain.Entities;

public class RoomRate
{
    public Guid Id { get; init; }
    public Guid RoomTypeId { get; init; }
    public string Name { get; private set; }
    public RoomRateType Type { get; private set; }
    public decimal PricePerNight { get;  private set; }
    public RateTimeInterval  TimeInterval { get; private set; }

    public RoomRate
    (
        Guid roomTypeId,
        string name,
        RoomRateType type,
        decimal pricePerNight,
        RateTimeInterval timeInterval
    )
    {
        if (roomTypeId == Guid.Empty)
            throw new EmptyRequiredFieldException("RoomId");
        
        if (string.IsNullOrWhiteSpace(name))
            throw new EmptyRequiredFieldException("Room rate name");
        
        if (Enum.IsDefined<RoomRateType>(type) is false)
            throw new BusinessRuleException("Invalid room rate type");
        
        if (pricePerNight < 0)
            throw new BusinessRuleException("Price per night cannot be negative");

        Id = Guid.CreateVersion7();
        RoomTypeId = roomTypeId;
        Name = name;
        Type = type;
        PricePerNight = pricePerNight;
        TimeInterval = timeInterval;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new EmptyRequiredFieldException("Room rate name");
        
        Name = newName;
    }

    public void UpdateType(RoomRateType newType)
    {
        if (Enum.IsDefined<RoomRateType>(newType) is false)
            throw new BusinessRuleException("Invalid room rate type");
        
        Type = newType;
    }

    public void UpdatePricePerNight(decimal newPricePerNight)
    {
        if (newPricePerNight < 0)
            throw new BusinessRuleException("Price per night cannot be negative");
        
        PricePerNight = newPricePerNight;
    }

    public void UpdateRateTimeInterval(RateTimeInterval newRateTimeInterval)
        =>  TimeInterval = newRateTimeInterval;
}