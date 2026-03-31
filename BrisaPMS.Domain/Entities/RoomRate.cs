using BrisaPMS.Domain.Enums;
using BrisaPMS.Domain.Exceptions;
using BrisaPMS.Domain.ValueObjects;

namespace BrisaPMS.Domain.Entities;

public class RoomRate
{
    public Guid Id { get; init; }
    public Guid RoomId { get; init; }
    public string Name { get; private set; }
    public RoomRateType Type { get; private set; }
    public decimal PricePerNight { get;  private set; }
    public RateTimeInterval  TimeInterval { get; private set; }

    public RoomRate(Guid roomId,
        string name,
        RoomRateType type,
        decimal pricePerNight,
        RateTimeInterval timeInterval
    )
    {
        if (roomId == Guid.Empty)
            throw new EmptyRequiredFieldException("RoomId");
        
        if (string.IsNullOrWhiteSpace(name))
            throw new EmptyRequiredFieldException("Room rate name");
        
        if (Enum.IsDefined<RoomRateType>(type) is false)
            throw new BusinessRuleException("Invalid room rate type");
        
        if (pricePerNight < 0)
            throw new BusinessRuleException("Price per night cannot be negative");

        Id = Guid.CreateVersion7();
        RoomId = roomId;
        Name = name;
        Type = type;
        PricePerNight = pricePerNight;
        TimeInterval = timeInterval;
    }
    
    
}