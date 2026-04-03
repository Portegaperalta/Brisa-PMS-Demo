using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Domain.Rooms;

public class RoomRate
{
    public Guid Id { get; init; }
    public Guid RoomTypeId { get; init; }
    public string Name { get; private set; }
    public RoomRateType Type { get; private set; }
    public Money PricePerNight { get;  private set; }
    public RoomRateTimeInterval  TimeInterval { get; private set; }

    public RoomRate
    (
        Guid roomTypeId,
        string name,
        RoomRateType type,
        Money pricePerNight,
        RoomRateTimeInterval timeInterval
    )
    {
        if (roomTypeId == Guid.Empty)
            throw new EmptyRequiredFieldException("RoomId");
        
        if (string.IsNullOrWhiteSpace(name))
            throw new EmptyRequiredFieldException("Room rate name");
        
        if (Enum.IsDefined<RoomRateType>(type) is false)
            throw new BusinessRuleException("Invalid room rate type");

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

    public void UpdatePricePerNight(Money newPricePerNight) =>
        PricePerNight = newPricePerNight;

    public void UpdateRateTimeInterval(RoomRateTimeInterval newRateTimeInterval)
        =>  TimeInterval = newRateTimeInterval;
}